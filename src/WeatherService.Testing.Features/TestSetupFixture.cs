using WeatherService.Testing.Core.Specifications;

namespace WeatherService.Testing.Core;

[SetUpFixture]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestSetup.Equivalency();
    }
}