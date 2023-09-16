using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Api.Features.AuditLogs;

internal static class AuditLogMapper
{
    public static Representation.AuditLog ToRepresentation(this AuditLog auditLog)
    { 
        return new Representation.AuditLog
        {
            AuditLogId = auditLog.AuditLogId,
            NameIdentifier = auditLog.NameIdentifier,
            Message = "blah",//auditLog.Message.Value(),
            TimeStampUTC = auditLog.TimeStampUTC,
        };
    }
}