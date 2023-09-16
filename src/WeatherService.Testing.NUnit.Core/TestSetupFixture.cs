using WeatherService.Testing.NUnit.Core.Specifications;

namespace WeatherService.Testing.NUnit.Core;

[SetUpFixture]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestSetup.Equivalency();
    }
}