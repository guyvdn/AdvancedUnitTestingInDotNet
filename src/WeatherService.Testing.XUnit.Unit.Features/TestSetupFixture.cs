using WeatherService.Testing.XUnit.Unit.Core.Equivalency;

namespace WeatherService.Testing.XUnit.Unit.Features;

public static class TestSetupFixture
{
    private static readonly object Locker = new();

    private static bool _executed;
    public static void Execute()
    {
        lock (Locker)
        {
            if (_executed)
                return;

            TestSetup.Equivalency<Core.AssemblyMarker>();
            TestSetup.Equivalency<AssemblyMarker>();

            _executed = true;
        }
    }
}