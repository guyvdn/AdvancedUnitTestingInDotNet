using WeatherService.Testing.NUnit.Integration.Core;
using WeatherService.Testing.NUnit.Integration.Core.Infrastructure.Database;

namespace WeatherService.Testing.NUnit.Integration.Seeding;

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