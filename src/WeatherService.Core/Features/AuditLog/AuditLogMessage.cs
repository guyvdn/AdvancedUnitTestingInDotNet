namespace WeatherService.Core.Features.AuditLog;

internal readonly record struct AuditLogMessage
{
    private readonly string _message;

    internal AuditLogMessage(string message)
    {
        _message = message;
    }

    public override string ToString() => _message;

    public static implicit operator string(AuditLogMessage value) => value.ToString();

    public static AuditLogMessage GetCurrentWeatherWasCalledForCity(string? city) => new($"Get current weather was called for city '{city}'");
}