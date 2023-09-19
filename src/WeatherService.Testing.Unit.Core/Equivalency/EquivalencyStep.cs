using FluentAssertions.Equivalency;

namespace WeatherService.Testing.Unit.Core.Equivalency;

public abstract class EquivalencyStep<TSubject, TExpectation> : IEquivalencyStep
{
    public EquivalencyResult Handle(
        Comparands comparands,
        IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        if (comparands.Subject is TSubject subject &&
            comparands.Expectation is TExpectation expectation)
        {
            Handle(subject, expectation, context, nestedValidator);
            return EquivalencyResult.AssertionCompleted;
        }

        return EquivalencyResult.ContinueWithNext;
    }

    protected abstract void Handle(
        TSubject subject,
        TExpectation expectation,
        IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator);
}