namespace WeatherService.Core.Infrastructure;

public static class UniqueId
{
    public static Guid NewGuid => GetNewGuid();

    internal static Func<Guid> GetNewGuid { get; set; } = Guid.NewGuid;
}