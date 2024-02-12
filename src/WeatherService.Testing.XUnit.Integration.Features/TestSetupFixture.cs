using WeatherService.Testing.XUnit.Integration.Core;
using WeatherService.Testing.XUnit.Unit.Core.Equivalency;

namespace WeatherService.Testing.XUnit.Integration.Features;

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
            TestSetup.Equivalency<AssemblyMarker>();
            _executed = true;
        }

        Semaphore.Release();
    }
}