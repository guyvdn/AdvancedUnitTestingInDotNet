using WeatherService.Testing.Integration.Core;
using WeatherService.Testing.Integration.Core.Infrastructure.Database;

namespace WeatherService.Testing.Integration.Seeding;

[SetUpFixture]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        DatabaseContext.Current = new LocalDbContext();
        await DatabaseContext.Current.CreateAsync();
    }

    [OneTimeTearDown]
    public async Task RunAfterAllTestsHaveCompleted()
    {
        await DatabaseContext.Current.DeleteAsync();
    }
}