using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Verify.MongoDB;

public static class MongoDBRecording
{
    public static void EnableRecording(this MongoClientSettings mongoClientSettings, MongoDBEvents events = MongoDBEvents.All)
    {
        mongoClientSettings.ClusterConfigurator = builder =>
        {
            var log = new LogCommandInterceptor();

            if ((events & MongoDBEvents.Started) == MongoDBEvents.Started)
            {
                builder.Subscribe<CommandStartedEvent>(@event => log.Command(@event));
            }

            if ((events & MongoDBEvents.Succeeded) == MongoDBEvents.Succeeded)
            {
                builder.Subscribe<CommandSucceededEvent>(@event => log.Command(@event));
            }

            if ((events & MongoDBEvents.Failed) == MongoDBEvents.Failed)
            {
                builder.Subscribe<CommandFailedEvent>(@event => log.Command(@event));
            }
        };
    }

    public static void StartRecording()
    {
        LogCommandInterceptor.Start();
    }

    public static IEnumerable<LogEntryBase> FinishRecording()
    {
        var entries = LogCommandInterceptor.Stop();
        if (entries is not null)
        {
            return entries;
        }
        throw new Exception("No recorded state. It is possible `MongoRecording.StartRecording()` has not been called on the MongoClient.");
    }
}