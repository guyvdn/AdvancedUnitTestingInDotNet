using FluentAssertions.Equivalency;
using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Testing.NUnit.Unit.Core.Equivalency;

internal sealed class StringToAuditLogMessageEquivalencyStep : EquivalencyStep<string, AuditLogMessage>
{
    protected override void Handle(
        string subject,
        AuditLogMessage expectation, 
        IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        subject.Should().BeEquivalentTo(expectation.ToString());
    }
}

internal sealed class AuditLogMessageToStringEquivalencyStep : EquivalencyStep<AuditLogMessage, string>
{
    protected override void Handle(
        AuditLogMessage subject,
        string expectation, 
        IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        subject.ToString().Should().BeEquivalentTo(expectation);
    }
}