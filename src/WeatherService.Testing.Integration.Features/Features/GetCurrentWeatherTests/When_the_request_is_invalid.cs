using WeatherService.Api.Features.WeatherForecasts;
using WeatherService.Core.Features.WeatherForecasts;
using WeatherService.Representation;
using WeatherService.Testing.Integration.Core;
using WeatherService.Testing.Integration.Core.Specifications;

namespace WeatherService.Testing.Integration.Features.Features.GetCurrentWeatherTests;

internal sealed class When_the_request_is_invalid
    : TestSpecification<WeatherForecastsController, GetCurrentWeather.Request, WeatherForecast>
{
    private HttpResponseMessage _response;

    protected override async Task ActAsync()
    {
        _response = await Client.GetAsync("WeatherForecasts");
    }

    [Test]
    public void It_should_return_the_correct_response()
    {
        _response.Content
            .Should().HaveProblemDetails()
            .And.ContainError(key: "City");
    }

    [Test]
    public void It_should_log_the_expected_messages()
    {
        TestSink.Should().Log("Error validating {Request} with {Failures}");
    }
}