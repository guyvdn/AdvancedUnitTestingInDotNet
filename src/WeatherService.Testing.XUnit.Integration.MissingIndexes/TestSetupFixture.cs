using WeatherService.Testing.XUnit.Integration.Core;

namespace WeatherService.Testing.XUnit.Integration.Seeding;

public static class TestSetupFixture
{
    private static readonly SemaphoreSlim Semaphore = new(1);

    private static bool _executed;
    public static async Task ExecuteAsync()
    {
        await Semaphore.WaitAsync();

        if (!_executed)
        {
            await DatabaseContext.Current.CreateAsync();
            _executed = true;
        }

        Semaphore.Release();
    }
}