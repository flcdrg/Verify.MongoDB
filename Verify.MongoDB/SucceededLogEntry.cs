using Argon;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver.Core.Connections;

namespace VerifyTests;

public class SucceededLogEntry : LogEntryBase
{
    [JsonIgnore]
    public TimeSpan Duration { get; }
    public JObject Document { get; }

    public SucceededLogEntry(
        string commandName,
        BsonDocument document,
        TimeSpan duration,
        ConnectionId connectionId,
        long? operationId,
        int requestId)
        : base("Succeeded", commandName, connectionId, operationId, requestId)
    {
        Duration = duration;

        var jsonWriterSettings = new JsonWriterSettings {OutputMode = JsonOutputMode.CanonicalExtendedJson };
        Document = JObject.Parse(document.ToJson(jsonWriterSettings));
    }
}