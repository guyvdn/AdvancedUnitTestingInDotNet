using WeatherService.Testing.Integration.Core.Infrastructure.Database;

namespace WeatherService.Testing.Integration.Core;

[SetUpFixture]
[SetCulture("nl")]
public sealed class TestSetupFixture
{
    public static IDatabaseContext DatabaseContext { get; } = new InMemoryDbContext();
    //public static IDatabaseContext DatabaseContext { get; } = new LocalDbContext();
    //public static IDatabaseContext DatabaseContext { get; } = new SqlDockerDbContext();

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        await DatabaseContext.CreateAsync();
        TestSetup.Equivalency();
    }

    [OneTimeTearDown]
    public async Task RunAfterAllTestsHaveCompleted()
    {
        await DatabaseContext.DeleteAsync();
    }
}