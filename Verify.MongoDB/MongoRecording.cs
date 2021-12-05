using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Verify.MongoDB;

public static class MongoRecording
{
    public static void EnableRecording(this MongoClientSettings mongoClientSettings, MongoEvents events = MongoEvents.All)
    {
        mongoClientSettings.ClusterConfigurator = builder =>
        {
            var log = new LogCommandInterceptor();

            if ((events & MongoEvents.Started) == MongoEvents.Started)
            {
                builder.Subscribe<CommandStartedEvent>(@event => log.Command(@event));
            }

            if ((events & MongoEvents.Succeeded) == MongoEvents.Succeeded)
            {
                builder.Subscribe<CommandSucceededEvent>(@event => log.Command(@event));
            }

            if ((events & MongoEvents.Failed) == MongoEvents.Failed)
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