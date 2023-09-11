using AutoFixture;

namespace WeatherService.Testing.NUnit.Core.Customizations;

internal static class FixtureExtensions
{
    public static Fixture WithCustomizations(this Fixture fixture)
    {
        //fixture.Register(Build.Bool);

        return fixture;
    }
}