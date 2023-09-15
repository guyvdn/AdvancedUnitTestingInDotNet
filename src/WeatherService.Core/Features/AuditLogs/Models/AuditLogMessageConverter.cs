using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WeatherService.Core.Features.AuditLogs.Models;

internal sealed class AuditLogMessageConverter : ValueConverter<AuditLogMessage, string>
{
    public AuditLogMessageConverter() : base(x => x.ToString(), x => new AuditLogMessage(x))
    {
    }
}