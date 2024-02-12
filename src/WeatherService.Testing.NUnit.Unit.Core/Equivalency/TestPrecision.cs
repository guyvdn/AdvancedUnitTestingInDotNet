namespace WeatherService.Testing.NUnit.Unit.Core.Equivalency;

public static class TestPrecision
{
    public static TimeSpan DateTimeEquals { get; }
        = TimeSpan.FromMilliseconds(value: 500);

    public static TimeSpan DateTimeCloseTo { get; }
        = TimeSpan.FromSeconds(value: 5);
}