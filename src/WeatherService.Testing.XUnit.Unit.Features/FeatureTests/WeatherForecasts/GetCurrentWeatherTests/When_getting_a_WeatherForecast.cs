using AutoFixture.AutoNSubstitute;
using Microsoft.AspNetCore.Http;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Exceptions;
using NSubstitute.Extensions;
using WeatherService.Core;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Core.Features.WeatherForecasts;
using WeatherService.Core.Features.WeatherForecasts.Models;
using WeatherService.Testing.XUnit.Unit.Core;

namespace WeatherService.Testing.XUnit.Unit.Features.FeatureTests.WeatherForecasts.GetCurrentWeatherTests;

public sealed class When_getting_a_WeatherForecast : TestSpecification
{
    private GetCurrentWeather.Request _request;
    private CancellationToken _cancellationToken;
    private WeatherResponse _currentWeather;
    private WeatherResponse _response;

    // Instead of using TestSpecification<TSut> we can use Freeze of AutoFixture
    protected override void Arrange()
    {
        var user = Fixture.Create("User");
        _request = Fixture.Create<GetCurrentWeather.Request>();
        _currentWeather = Fixture.Create<WeatherResponse>();
        _cancellationToken = Build.CancellationToken();

        Fixture.Customize(new AutoNSubstituteCustomization());

        // By using Freeze we make sure the same instance is returned always
        Fixture.Freeze<IHttpContextAccessor>()
            .HttpContext
            .Returns(Build.AuthenticatedHttpContext(user));

        // Strict mocking with NSubstitute
        Fixture.Freeze<IWeatherRepository>()
            .AddAuditLogAsync(Arg.Any<AuditLog>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(_ => new SubstituteException("No matching setup found for AddAuditLogAsync"));

        Fixture.Create<IWeatherRepository>()
            .Configure() // Is needed so that the next line will not throw here
            .AddAuditLogAsync(Arg.Is<AuditLog>(x => x.NameIdentifier == user), _cancellationToken)
            .Returns(Task.CompletedTask);

        // Loose mocking with NSubstitute
        Fixture.Freeze<IWeatherApiClient>()
            .GetCurrentWeatherAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(_currentWeather);
    }

    protected override async Task ActAsync()
    {
        var sut = Fixture.Create<GetCurrentWeather.Handler>();
        _response = await sut.Handle(_request, _cancellationToken);
    }

    [Fact]
    public void Should_return_the_correct_response()
    {
        _response.Should().NotBeNull();
    }

    [Fact]
    public void Should_call_the_api_with_the_correct_parameters()
    {
        // When using Loose mocking, strict validation is needed
        Fixture.Create<IWeatherApiClient>()
            .Received(1)
            .GetCurrentWeatherAsync(_request.City, _cancellationToken);
    }
}