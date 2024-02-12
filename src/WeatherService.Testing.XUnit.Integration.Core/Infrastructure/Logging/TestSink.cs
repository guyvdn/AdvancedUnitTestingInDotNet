using Serilog.Core;
using Serilog.Events;

namespace WeatherService.Testing.XUnit.Integration.Core.Infrastructure.Logging;

public sealed class TestSink : ILogEventSink
{
    public ICollection<LogEvent> Logs { get; } = new List<LogEvent>();

    public void PrintAllEvents()
    {
        foreach (var logEvent in Logs)
        {
            Console.WriteLine("- " + logEvent.RenderMessage());

            if (logEvent.Exception != null)
                Console.WriteLine(logEvent.Exception.Message);
        }

        Console.WriteLine();
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