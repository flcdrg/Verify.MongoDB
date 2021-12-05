namespace Verify.MongoDB;

[Flags]
public enum MongoDBEvents
{
    Started = 1,
    Succeeded = 2,
    Failed = 4,
    All = Started | Succeeded | Failed
}