using WeatherService.Testing.NUnit.Integration.Core;
using WeatherService.Testing.NUnit.Unit.Core.Equivalency;

namespace WeatherService.Testing.NUnit.Integration.Features;

[SetUpFixture]
[SetCulture("nl")]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        await DatabaseContext.Current.CreateAsync();
        TestSetup.Equivalency<AssemblyMarker>();
    }

    [OneTimeTearDown]
    public async Task RunAfterAllTestsHaveCompleted()
    {
        await DatabaseContext.Current.DeleteAsync();
    }
}