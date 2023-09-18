using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Features;

[SetUpFixture]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestSetup.Equivalency<AssemblyMarker>();
    }
}