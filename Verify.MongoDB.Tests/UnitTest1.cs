using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Verify.MongoDB.Tests;

[UsesVerify]
public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        VerifyMongoDb.Enable();

        var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(
            "mongodb://localhost:C2y6yDjf5%2FR%2Bob0N8A7Cgv30VRDJIWEHLM%2B4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw%2FJw%3D%3D@localhost:10255/admin?ssl=true"));

        clientSettings.EnableRecording();

        var client = new MongoClient(clientSettings);


        var database = client.GetDatabase("VerifyTests");

        var collection = database.GetCollection<BsonDocument>("docs");
        MongoRecording.StartRecording();

        await collection.InsertOneAsync(new BsonDocument());

        var result = await collection.FindAsync(Builders<BsonDocument>.Filter.Eq("_id", "blah"),
            new FindOptions<BsonDocument, BsonDocument>());
            
        await Verifier.Verify("collection");
    }
}