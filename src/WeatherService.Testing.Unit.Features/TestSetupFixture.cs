using WeatherService.Testing.Unit.Core.Specifications;

namespace WeatherService.Testing.Unit.Features;

[SetUpFixture]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestSetup.Equivalency<AssemblyMarker>();
    }
}