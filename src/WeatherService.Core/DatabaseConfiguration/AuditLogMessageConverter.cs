using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherService.Core.Features.AuditLog;

namespace WeatherService.Core.DatabaseConfiguration;

internal sealed class AuditLogMessageConverter : ValueConverter<AuditLogMessage, string>
{
    public AuditLogMessageConverter() : base(x => x.ToString(), x => new AuditLogMessage(x))
    {
    }
}