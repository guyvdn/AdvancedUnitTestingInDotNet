using Serilog.Core;
using Serilog.Events;

namespace WeatherService.Testing.Integration.Core.Infrastructure;

internal sealed class TestSink : ILogEventSink
{
    public static ICollection<LogEvent> Logs { get; } = new List<LogEvent>();

    public static void PrintAllEvents()
    {
        foreach (var logEvent in Logs)
        {
            TestContext.WriteLine("- " + logEvent.RenderMessage());

            if (logEvent.Exception != null)
                TestContext.WriteLine(logEvent.Exception.Message);
        }

        TestContext.WriteLine();
    }

    public static void ClearAllEvents()
    {
        Logs.Clear();
    }

    public void Emit(LogEvent logEvent)
    {
        Logs.Add(logEvent);
    }
}