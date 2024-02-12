namespace WeatherService.Testing.XUnit.Integration.Seeding;

public abstract class TestSpecification : WeatherService.Testing.XUnit.Integration.Core.Specifications.TestSpecification
{
    public override async Task InitializeAsync()
    {
        await TestSetupFixture.ExecuteAsync();
        await base.InitializeAsync();
    }
}