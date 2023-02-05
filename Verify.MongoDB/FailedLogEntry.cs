using Argon;
using MongoDB.Driver.Core.Connections;

namespace VerifyTests;

public class FailedLogEntry : LogEntryBase
{
    public Exception? Exception { get; }
    [JsonIgnore]
    public TimeSpan Duration { get; }

    public FailedLogEntry(string commandName, Exception exception, TimeSpan duration, ConnectionId connectionId, long? operationId, int requestId)
        : base("Failed", commandName, connectionId, operationId, requestId)
    {
        Exception = exception;
        Duration = duration;
    }
}