using FluentAssertions.Equivalency;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.Core.EquivalencySteps;

namespace WeatherService.Testing.Features.EquivalencySteps;

public class AuditLogMessageToStringEquivalencyStep : EquivalencyStep<AuditLogMessage, string>
{
    protected override void Handle(AuditLogMessage subject, string expectation, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        subject.ToString().Should().BeEquivalentTo(expectation);
    }
}

public class StringToAuditLogMessageEquivalencyStep : EquivalencyStep<string, AuditLogMessage>
{
    protected override void Handle(string subject, AuditLogMessage expectation, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        subject.Should().BeEquivalentTo(expectation.ToString());
    }
}