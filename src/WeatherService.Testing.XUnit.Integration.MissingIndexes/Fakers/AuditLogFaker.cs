using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Testing.XUnit.Integration.Seeding.Fakers;

public sealed class AuditLogFaker : NlFaker<AuditLog>
{
    public AuditLogFaker()
    {
        RuleFor(x => x.Message, f => new AuditLogMessage(f.Lorem.Letter(100)));
        RuleFor(x => x.TimeStampUTC, f => f.Date.Recent(14));
        RuleFor(x => x.NameIdentifier, f => f.Person.Email);
    }
}