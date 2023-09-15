using WeatherService.Testing.Integration.Core.Infrastructure;

namespace WeatherService.Testing.Integration.Core;

[SetUpFixture]
[SetCulture("nl")]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        LocalDbContext.Create();
        TestSetup.Equivalency();
    }

    [OneTimeTearDown]
    public void RunAfterAllTestsHaveCompleted()
    {
        LocalDbContext.Cleanup();
    }
}