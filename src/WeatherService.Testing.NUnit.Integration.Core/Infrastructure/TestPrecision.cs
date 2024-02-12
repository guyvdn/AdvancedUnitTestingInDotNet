namespace WeatherService.Testing.NUnit.Integration.Core.Infrastructure;

public static class TestPrecision
{
    public static TimeSpan DateTimeEquals { get; } = TimeSpan.FromMilliseconds(value: 500);

    public static TimeSpan DateTimeCloseTo { get; } = TimeSpan.FromSeconds(value: 5);
}