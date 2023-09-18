using FluentAssertions.Equivalency;

namespace WeatherService.Testing.Core.Specifications;

public static class TestSetup
{
    public static void Equivalency<TAssemblyMarker>()
    {
        var equivalencySteps = typeof(TAssemblyMarker)
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
        options.Using<DateTime>(x => x.Subject.Should()
            .BeCloseTo(x.Expectation, TestPrecision.DateTimeEquals))
            .WhenTypeIs<DateTime>();

        return options;
    }
}

