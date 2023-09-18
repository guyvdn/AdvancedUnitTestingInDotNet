using Microsoft.Extensions.Logging;

namespace WeatherService.Testing.Core.Fakes;

public abstract class FakeLogger
{
    public ICollection<MessageEntry> Messages { get; } = new List<MessageEntry>();
}

public sealed class FakeLogger<T> : FakeLogger, ILogger<T>
{
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        return new Scope<TState>(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);

        var messageEntry = new MessageEntry
        {
            LogLevel = logLevel,
            Message = message,
            EventId = eventId,
        };

        Messages.Add(messageEntry);
    }

    private sealed class Scope<TState> : IDisposable
    {
        public Scope(TState state)
        {
            State = state;
        }

        public TState State { get; init; }

        public void Dispose()
        {
            // Nothing to Dispose
        }
    }
}

public sealed class MessageEntry
{
    public required LogLevel LogLevel { get; set; }

    public required string Message { get; set; }

    public required EventId EventId { get; set; }
}