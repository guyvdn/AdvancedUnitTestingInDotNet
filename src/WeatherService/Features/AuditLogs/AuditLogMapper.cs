using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Api.Features.AuditLogs;

public static class AuditLogMapper
{
    public static Representation.AuditLog ToRepresentation(this AuditLog auditLog)
    { 
        return new Representation.AuditLog
        {
            NameIdentifier = auditLog.NameIdentifier,
            Message = auditLog.Message,
            TimeStampUTC = auditLog.TimeStampUTC,
        };
    }
}