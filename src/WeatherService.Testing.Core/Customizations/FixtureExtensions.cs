namespace WeatherService.Testing.Core.Customizations;

internal static class FixtureExtensions
{
    public static Fixture WithCustomizations(this Fixture fixture)
    {
        fixture.Register(Build.Bool);

        fixture.Customize(new CurrentWeatherCustomization());

        fixture.Customizations.Add(new Base64StringCustomization());

        return fixture;
    }
}