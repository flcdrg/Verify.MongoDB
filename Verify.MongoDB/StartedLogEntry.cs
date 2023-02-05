using Argon;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver.Core.Connections;

namespace VerifyTests;

public class StartedLogEntry : LogEntryBase
{
    public string? Database { get; }
    public JObject? Document { get; }

    public StartedLogEntry(
        string commandName,
        BsonDocument document,
        string databaseName,
        ConnectionId connectionId,
        long? operationId,
        int requestId)
        : base("Started", commandName, connectionId, operationId, requestId)
    {
        Database = databaseName;

        var jsonWriterSettings = new JsonWriterSettings {OutputMode = JsonOutputMode.Strict};
        Document = JObject.Parse(document.ToJson(jsonWriterSettings));
    }
}