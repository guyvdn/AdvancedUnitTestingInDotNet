using System.Net.Http.Json;
using RichardSzalay.MockHttp;
using WeatherService.Api.Features.WeatherForecast;
using WeatherService.Core.Features.WeatherForecasts;
using WeatherService.Core.Features.WeatherForecasts.Models;
using WeatherService.Representation;
using WeatherService.Testing.Integration.Core.Infrastructure;

namespace WeatherService.Testing.Integration.Core.Features.GetCurrentWeatherTests;

internal sealed class When_all_is_good : TestSpecification<WeatherForecastsController, GetCurrentWeather.Request>
{
    private WeatherForecast? _response;

    protected override void Arrange()
    {
        var response = Fixture.Create<WeatherResponse>();

        HttpMessageHandler
            .Expect("https://weatherapi/v1/current.json")
            .WithQueryString("key", "ApiKey")
            .WithQueryString("q", "Belgium")
            .Respond(JsonContent.Create(response));
    }

    protected override async Task ActAsync()
    {
        _response = await Client.GetFromJsonAsync<WeatherForecast>("WeatherForecasts");
    }

    [Test]
    public void It_should_return_the_correct_response()
    {
        _response.Should().NotBeNull();
    }
}