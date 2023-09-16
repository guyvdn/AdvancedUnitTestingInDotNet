namespace WeatherService.Testing.NUnit.Core.Specifications;

public static class TestPrecision
{
    public static TimeSpan DateTimeEquals { get; } = TimeSpan.FromMilliseconds(value: 500);

    public static TimeSpan DateTimeCloseTo { get; } = TimeSpan.FromSeconds(value: 5);
}