using WeatherService.Api.Features.WeatherForecasts;
using WeatherService.Core.Features.WeatherForecasts;
using WeatherService.Representation;
using WeatherService.Testing.NUnit.Integration.Core;
using WeatherService.Testing.NUnit.Integration.Core.Specifications;

namespace WeatherService.Testing.NUnit.Integration.Features.Features.GetCurrentWeatherTests;

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
        // Assert the ProblemDetails
        _response.Content
            .Should().HaveProblemDetails()
            .And.ContainError(key: "City");
    }
}