﻿namespace WeatherService.Testing.Integration.Core;

[SetUpFixture]
[SetCulture("nl")]
public sealed class TestSetupFixture
{
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        await DatabaseContext.Current.CreateAsync();
        TestSetup.Equivalency();
    }

    [OneTimeTearDown]
    public async Task RunAfterAllTestsHaveCompleted()
    {
        await DatabaseContext.Current.DeleteAsync();
    }
}