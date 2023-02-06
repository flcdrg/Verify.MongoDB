using EphemeralMongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Verify.MongoDB.Tests;

[UsesVerify]
public class IntegrationTests : IDisposable
{
    private IMongoRunner mongoRunner;

    static IntegrationTests()
    {
        VerifyMongoDb.Enable();
    }

    public IntegrationTests()
    {
        mongoRunner = MongoRunner.Run();
    }

    public void Dispose()
    {
        mongoRunner?.Dispose();
    }

    [Fact]
    public async Task FindAsync()
    {
        var clientSettings = MongoClientSettings.FromConnectionString(mongoRunner.ConnectionString);

        clientSettings.EnableRecording();

        var client = new MongoClient(clientSettings);

        var database = client.GetDatabase("VerifyTests");

        await database.DropCollectionAsync("docs");

        var collection = database.GetCollection<BsonDocument>("docs");

        MongoDBRecording.StartRecording();

        await collection.FindAsync(Builders<BsonDocument>.Filter.Eq("_id", "blah"),
            new FindOptions<BsonDocument, BsonDocument>());

        await Verifier.Verify("collection");
    }

    [Fact]
    public async Task InsertOneAsync()
    {
        var clientSettings = MongoClientSettings.FromConnectionString(mongoRunner.ConnectionString);

        clientSettings.EnableRecording();

        var client = new MongoClient(clientSettings);

        var database = client.GetDatabase("VerifyTests");

        await database.DropCollectionAsync("docs");

        var collection = database.GetCollection<BsonDocument>("docs");

        MongoDBRecording.StartRecording();

        await collection.InsertOneAsync(new()
        {
            {"_id", "C2E1B774-A997-4818-B104-E915F7DCA9C1"}
        });

        await Verifier.Verify("collection");
    }
}