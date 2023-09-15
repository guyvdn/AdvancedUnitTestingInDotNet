using AutoFixture;
using WeatherService.Testing.NUnit.Core.Customizations;

namespace WeatherService.Testing.NUnit.Core.Specifications;

public abstract class TestSpecificationBase
{
    protected static Fixture Fixture { get; }

    static TestSpecificationBase()
    {
        Fixture = new Fixture().WithCustomizations();
    }
}