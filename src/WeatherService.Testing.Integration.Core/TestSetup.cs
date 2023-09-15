using FluentAssertions.Equivalency;
using WeatherService.Testing.Integration.Core.Infrastructure;

namespace WeatherService.Testing.Integration.Core;

internal static class TestSetup
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

    private static EquivalencyAssertionOptions EquivalencyDefaults(EquivalencyAssertionOptions o)
    {
        o.ExcludingMissingMembers();

        o.Using<DateTime>(x => x.Subject.Should().BeCloseTo(x.Expectation, TestPrecision.DateTimeEquals)).WhenTypeIs<DateTime>();

        return o;
    }
}