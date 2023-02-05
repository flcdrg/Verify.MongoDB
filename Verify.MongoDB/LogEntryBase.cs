using Argon;
using MongoDB.Driver.Core.Connections;

namespace VerifyTests;

public abstract class LogEntryBase
{
    public string Type { get; }
    public string Command { get; }
    public DateTimeOffset StartTime { get; }
    [JsonIgnore]
    public ConnectionId ConnectionId { get; }
    public long? OperationId { get; }
    public int RequestId { get; }

    protected LogEntryBase(string type, string command, ConnectionId connectionId, long? operationId, int requestId)
    {
        Type = type;
        Command = command;
        ConnectionId = connectionId;
        OperationId = operationId;
        RequestId = requestId;
        StartTime = DateTimeOffset.Now;
    }
}