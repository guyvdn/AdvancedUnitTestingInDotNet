using System.Security.Claims;
using AutoFixture.AutoNSubstitute;
using Microsoft.AspNetCore.Http;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Exceptions;
using NSubstitute.Extensions;
using WeatherService.Core;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Core.Features.WeatherForecasts;
using WeatherService.Core.Features.WeatherForecasts.Models;
using WeatherService.Testing.Unit.Core;
using WeatherService.Testing.Unit.Core.Specifications;

namespace WeatherService.Testing.Unit.Features.FeatureTests.WeatherForecasts.GetCurrentWeatherTests;

internal sealed class When_getting_a_WeatherForecast : TestSpecification
{
    private GetCurrentWeather.Request _request;
    private CancellationToken _cancellationToken;
    private WeatherResponse _currentWeather;
    private WeatherResponse _response;

    protected override void Arrange()
    {
        var user = Fixture.Create("User");
        _request = Fixture.Create<GetCurrentWeather.Request>();
        _currentWeather = Fixture.Create<WeatherResponse>();
        _cancellationToken = Build.CancellationToken();

        Fixture.Customize(new AutoNSubstituteCustomization());

        Fixture.Freeze<IHttpContextAccessor>()
            .HttpContext
            .Returns(CreateAuthenticatedHttpContext(user));

        // Strict mocking with NSubstitute
        Fixture.Freeze<IWeatherRepository>()
            .AddAuditLogAsync(Arg.Any<AuditLog>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(_ => new SubstituteException("No matching setup found for AddAuditLogAsync"));

        Fixture.Create<IWeatherRepository>()
            .Configure()
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

    [Test]
    public void Should_return_the_correct_response()
    {
        _response.Should().NotBeNull();
    }

    [Test]
    public void Should_call_the_api_with_the_correct_parameters()
    {
        Fixture.Create<IWeatherApiClient>()
            .Received(1)
            .GetCurrentWeatherAsync(_request.City, _cancellationToken);
    }

    private static DefaultHttpContext CreateAuthenticatedHttpContext(string user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user)
        };

        var identity = new ClaimsIdentity(claims);

        return new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity)
        };
    }
}