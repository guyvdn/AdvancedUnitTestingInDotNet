using WeatherService.Core.Features.WeatherForecasts.Models;

namespace WeatherService.Testing.NUnit.Core.Customizations;

internal static class FixtureExtensions
{
    public static Fixture WithCustomizations(this Fixture fixture)
    {
        fixture.Register(Build.Bool);

        fixture.Customize(new CurrentWeatherCustomization());

        return fixture;
    }
}

public class CurrentWeatherCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        // WeatherApi example return value: "2023-09-17 12:00"
        fixture.Customize<CurrentWeather>(c =>
            c.With(x => x.LastUpdated, Build.DateTime().ToString("YYYY-MM-dd hh:mm"))
        );
    }
}