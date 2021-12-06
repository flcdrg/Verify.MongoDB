using System.Collections.Concurrent;
using MongoDB.Driver.Core.Events;

namespace VerifyTests;

public class LogCommandInterceptor
{
    private static readonly AsyncLocal<State?> asyncLocal = new();

    public static void Start()
    {
        asyncLocal.Value = new();
    }

    public static IEnumerable<LogEntryBase>? Stop()
    {
        var state = asyncLocal.Value;
        asyncLocal.Value = null;
        return state?.Events.OrderBy(x => x.StartTime);
    }

    public void Command(CommandStartedEvent @event)
    {
        asyncLocal.Value?.WriteLine(new StartedLogEntry(@event.CommandName, @event.Command, @event.DatabaseNamespace.DatabaseName, @event.ConnectionId, @event.OperationId, @event.RequestId));
    }

    public void Command(CommandSucceededEvent @event)
    {
        asyncLocal.Value?.WriteLine(new SucceededLogEntry(@event.CommandName, @event.Reply, @event.Duration, @event.ConnectionId, @event.OperationId, @event.RequestId));
    }

    public void Command(CommandFailedEvent @event)
    {
        asyncLocal.Value?.WriteLine(new FailedLogEntry(@event.CommandName, @event.Failure, @event.Duration, @event.ConnectionId, @event.OperationId, @event.RequestId ));
    }
    private class State
    {
        internal readonly ConcurrentQueue<LogEntryBase> Events = new();

        public void WriteLine(LogEntryBase entry)
        {
            Events.Enqueue(entry);
        }
    }
}