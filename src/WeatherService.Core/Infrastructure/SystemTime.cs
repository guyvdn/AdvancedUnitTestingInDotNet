namespace WeatherService.Core.Infrastructure;

public static class SystemTime
{
    public static DateTime UtcNow => GetUtcNow();

    public static DateTimeOffset OffsetUtcNow => GetOffsetUtcNow();

    internal static Func<DateTime> GetUtcNow { get; set; } = () => DateTime.UtcNow;

    internal static Func<DateTimeOffset> GetOffsetUtcNow { get; set; } = () => DateTimeOffset.UtcNow;
}