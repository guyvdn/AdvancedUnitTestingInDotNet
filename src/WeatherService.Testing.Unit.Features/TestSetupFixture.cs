using WeatherService.Testing.Unit.Core.Equivalency;

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