using WeatherService.Core.Features.WeatherForecasts.Models;

namespace WeatherService.Testing.NUnit.Unit.Core.Customizations;

public class CurrentWeatherCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        // WeatherApi example return value: "2023-09-17 12:00"
        fixture.Customize<CurrentWeather>(c =>
            c.With(x => x.LastUpdated, Build.DateTime().ToString("yyyy-MM-dd hh:mm"))
        );
    }
}