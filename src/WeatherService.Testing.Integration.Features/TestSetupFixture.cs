using WeatherService.Testing.Integration.Core;
using WeatherService.Testing.Unit.Core.Specifications;

namespace WeatherService.Testing.Integration.Features;

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