using WeatherService.Testing.NUnit.Unit.Core.Equivalency;

namespace WeatherService.Testing.NUnit.Unit.Features;

[SetUpFixture]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestSetup.Equivalency<Testing.NUnit.Unit.Core.AssemblyMarker>();
        TestSetup.Equivalency<AssemblyMarker>();
    }
}