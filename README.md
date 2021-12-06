# Verify.MongoDB

[![.NET](https://github.com/flcdrg/Verify.MongoDB/actions/workflows/dotnet.yml/badge.svg)](https://github.com/flcdrg/Verify.MongoDB/actions/workflows/dotnet.yml)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of MongoDB bits.

Verify.MongoDB is heavily inspired by @SimonCropp's [Verify.EntityFramework](https://github.com/VerifyTests/Verify.EntityFramework)

## Enable

Enable VerifyMongoDb once at assembly load time:

```csharp
VerifyMongoDb.Enable();
```

## Recording

Recording allows all commands executed by the MongoDB driver to be captured and then (optionally) verified.

Call `MongoDbRecording.EnableRecording()` on `MongoClientSettings`.

```csharp
var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(configuration["MongoConnectionString"]));

clientSettings.EnableRecording();
```

By default, all three event types (Started, Succeeded and Failed) are recorded. You can optionally specify the events required.

```csharp
clientSettings.EnableRecording(MongoDbEvents.Succeeded | MongoDbEvents.Failed);
```

`EnableRecording` should only be called in the test context.

### Usage

To start recording call `MongoDbRecording.StartRecording()`. The results will be automatically included in verified file.

```csharp
MongoDBRecording.StartRecording();

await collection.FindAsync(Builders<BsonDocument>.Filter.Eq("_id", "blah"),
    new FindOptions<BsonDocument, BsonDocument>());
    
await Verifier.Verify("collection");
```

Will result in the following verified file:

```txt
{
  target: collection,
  mongo: [
    {
      Database: VerifyTests,
      Document: {
        filter: {
          _id: blah
        },
        find: docs
      },
      Type: Started,
      Command: find,
      StartTime: DateTimeOffset_1,
      OperationId: Id_1,
      RequestId: Id_2
    },
    {
      Document: {
        cursor: {
          firstBatch: [],
          id: 0,
          ns: VerifyTests.docs
        },
        ok: 1.0
      },
      Type: Succeeded,
      Command: find,
      StartTime: DateTimeOffset_2,
      OperationId: Id_1,
      RequestId: Id_2
    }
  ]
}
```
