using System.Collections.Concurrent;
using MongoDB.Bson;
using MongoDB.Driver.Core.Events;

namespace VerifyTests;

public class LogCommandInterceptor
{
    private static readonly AsyncLocal<State?> asyncLocal = new();

    public static void Start()
    {
        asyncLocal.Value = new State();
    }

    public static IEnumerable<LogEntry>? Stop()
    {
        var state = asyncLocal.Value;
        asyncLocal.Value = null;
        return state?.Events.OrderBy(x => x.StartTime);
    }

    public void Command(CommandStartedEvent @event)
    {
        asyncLocal.Value?.WriteLine(new LogEntry(@event.CommandName, @event.Command, @event.DatabaseNamespace.DatabaseName));
    }

    public void Command(CommandSucceededEvent @event)
    {
        asyncLocal.Value?.WriteLine(new LogEntry(@event.CommandName, @event.Reply, string.Empty));
    }

    private class State
    {
        internal readonly ConcurrentQueue<LogEntry> Events = new();

        public void WriteLine(LogEntry entry)
        {
            Events.Enqueue(entry);
        }
    }
}