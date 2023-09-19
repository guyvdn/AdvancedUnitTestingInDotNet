namespace WeatherService.Core.Features.AuditLogs.Models;

public sealed class AuditLog
{
    public int AuditLogId { get; set; }

    public required string NameIdentifier { get; set; }

    public required AuditLogMessage Message { get; set; }

    public required DateTime TimeStampUTC { get; set; }
}