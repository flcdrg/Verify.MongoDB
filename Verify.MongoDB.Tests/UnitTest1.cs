using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", false)
            .Build();

        VerifyMongoDb.Enable();

        var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(configuration["MongoConnectionString"]));

        clientSettings.EnableRecording();

        var client = new MongoClient(clientSettings);

        var database = client.GetDatabase("VerifyTests");

        await database.DropCollectionAsync("docs");

        var collection = database.GetCollection<BsonDocument>("docs");

        MongoRecording.StartRecording();

        await collection.InsertOneAsync(new BsonDocument()
        {
            {"_id", "C2E1B774-A997-4818-B104-E915F7DCA9C1"}
        });

        var result = await collection.FindAsync(Builders<BsonDocument>.Filter.Eq("_id", "blah"),
            new FindOptions<BsonDocument, BsonDocument>());
            
        await Verifier.Verify("collection");
    }
}