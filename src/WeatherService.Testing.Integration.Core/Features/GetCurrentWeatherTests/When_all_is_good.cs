using System.Net.Http.Json;
using FluentAssertions;
using WeatherService.Api;
using WeatherService.Api.Controllers;
using WeatherService.Core.Features.CurrentWeather;
using WeatherService.Testing.Integration.Core.Infrastructure;

namespace WeatherService.Testing.Integration.Core.Features.GetCurrentWeatherTests;

internal sealed class When_all_is_good : TestSpecification<WeatherForecastController, GetCurrentWeather.Request>
{
    private WeatherForecast? _response;

    protected override async Task ActAsync()
    {
        _response = await Client.GetFromJsonAsync<WeatherForecast>("WeatherForecast");
    }

    [Test]
    public void METHOD()
    {
        _response.Should().NotBeNull();
    }
}