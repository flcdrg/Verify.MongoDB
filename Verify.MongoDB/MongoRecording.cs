using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Verify.MongoDB;

public static class MongoRecording
{
    public static void EnableRecording(this MongoClientSettings mongoClientSettings)
    {
        mongoClientSettings.ClusterConfigurator = builder =>
        {
            var log = new LogCommandInterceptor();
            builder.Subscribe<CommandStartedEvent>(@event => log.Command(@event));
        };
    }

    public static void StartRecording()
    {
        LogCommandInterceptor.Start();
    }

    public static IEnumerable<LogEntry> FinishRecording()
    {
        var entries = LogCommandInterceptor.Stop();
        if (entries is not null)
        {
            return entries;
        }
        throw new Exception("No recorded state. It is possible `MongoRecording.StartRecording()` has not been called on the MongoClient.");
    }
}