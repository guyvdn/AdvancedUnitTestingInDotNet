using FluentAssertions.Equivalency;
using WeatherService.Core.Features.AuditLogs.Models;
using WeatherService.Testing.Unit.Core.EquivalencySteps;

namespace WeatherService.Testing.Unit.Features.EquivalencySteps;

internal sealed class AuditLogMessageToStringEquivalencyStep : EquivalencyStep<AuditLogMessage, string>
{
    protected override void Handle(AuditLogMessage subject, string expectation, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        subject.ToString().Should().BeEquivalentTo(expectation);
    }
}

internal sealed class StringToAuditLogMessageEquivalencyStep : EquivalencyStep<string, AuditLogMessage>
{
    protected override void Handle(string subject, AuditLogMessage expectation, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        subject.Should().BeEquivalentTo(expectation.ToString());
    }
}