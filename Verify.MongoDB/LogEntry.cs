using System.Data.Common;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VerifyTests;

public class LogEntry
{
    public LogEntry(string type, BsonDocument document, string database)
    {
        Type = type;
        Database = database;

        var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
        Document = JObject.Parse(document.ToJson<MongoDB.Bson.BsonDocument>(jsonWriterSettings));
        StartTime = DateTimeOffset.Now;
    }

    public string Type { get; }
    public string Database { get; }
    public JObject Document { get; }
    public DateTimeOffset StartTime { get; }
}