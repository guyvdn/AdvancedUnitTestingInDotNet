using Serilog.Core;
using Serilog.Events;

namespace WeatherService.Testing.NUnit.Integration.Core.Infrastructure.Logging;

public sealed class TestSink : ILogEventSink
{
    public ICollection<LogEvent> Logs { get; } = new List<LogEvent>();

    public void PrintAllEvents()
    {
        foreach (var logEvent in Logs)
        {
            TestContext.WriteLine("- " + logEvent.RenderMessage());

            if (logEvent.Exception != null)
                TestContext.WriteLine(logEvent.Exception.Message);
        }

        TestContext.WriteLine();
    }

    public void ClearAllEvents()
    {
        Logs.Clear();
    }

    public void Emit(LogEvent logEvent)
    {
        Logs.Add(logEvent);
    }
}