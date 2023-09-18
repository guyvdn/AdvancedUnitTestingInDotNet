using FluentAssertions.Equivalency;
using WeatherService.Core.Features.AuditLogs.Models;

namespace WeatherService.Testing.Core.Specifications;

public static class TestSetup
{
    public static void Equivalency()
    {
        var equivalencySteps = typeof(AssemblyMarker)
            .Assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(IEquivalencyStep)));

        foreach (var equivalencyStep in equivalencySteps)
        {
            var methodInfo = typeof(EquivalencyPlan).GetMethod(nameof(EquivalencyPlan.Insert));
            var generic = methodInfo!.MakeGenericMethod(equivalencyStep);
            generic.Invoke(AssertionOptions.EquivalencyPlan, parameters: null);
        }

        AssertionOptions.AssertEquivalencyUsing(EquivalencyDefaults);
    }

    private static EquivalencyAssertionOptions EquivalencyDefaults(EquivalencyAssertionOptions options)
    {
        options.ExcludingMissingMembers();

        options.WithAutoConversion();

        options.Using<DateTime>(x => x.Subject.Should().BeCloseTo(x.Expectation, TestPrecision.DateTimeEquals)).WhenTypeIs<DateTime>();

        return options;
    }
}


public abstract class EquivalencyStep<TSubject, TExpectation> : IEquivalencyStep
{
    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
    {
        if (comparands.Subject is TSubject subject &&
            comparands.Expectation is TExpectation expectation)
        {
            Handle(subject, expectation, context, nestedValidator);
            return EquivalencyResult.AssertionCompleted;
        }

        return EquivalencyResult.ContinueWithNext;
    }

    protected abstract void Handle(TSubject subject, TExpectation expectation, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator);
}

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