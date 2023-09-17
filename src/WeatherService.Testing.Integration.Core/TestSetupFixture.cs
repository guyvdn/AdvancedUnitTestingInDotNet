namespace WeatherService.Testing.Integration.Core;

[SetUpFixture]
[SetCulture("nl")]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestDatabaseContext.Create();
        TestSetup.Equivalency();
    }

    [OneTimeTearDown]
    public void RunAfterAllTestsHaveCompleted()
    {
        TestDatabaseContext.Delete();
    }
}