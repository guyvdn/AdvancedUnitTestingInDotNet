namespace WeatherService.Core.Features.AuditLog;

internal sealed class AuditLog
{
    public int AuditLogId { get; protected set; }

    public required string NameIdentifier { get; set; }

    public required AuditLogMessage Message { get; set; }

    public required DateTime TimeStampUTC { get; set; }
}