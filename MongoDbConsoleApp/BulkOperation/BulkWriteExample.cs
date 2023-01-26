using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBConsoleApp.BulkOperation;

public class BulkWriteExample
{
    public void ExampleOne(MongoClient client)
    {
        /***
         * Document as Dictionary class 
         */
        Dictionary<string, Object> obj = new Dictionary<string, object>()
        {
            {"title", "Moongose"},
            {"authors", BsonArray.Create(new[] {"Adam", "Karol"})}
        };
        /***
         * Inserts for bulk writes
         */
        var inserts = new InsertOneModel<BsonDocument>(BsonDocument.Create(obj));
            
        /***
         * Update operations
         */
        var updates = new UpdateOneModel<BsonDocument>(
            Builders<BsonDocument>.Filter.Eq("title", "C#"),
            Builders<BsonDocument>.Update.Set("created", DateTime.Now));
        /***
         * List of all bulk operations
         */
        List <WriteModel<BsonDocument>> writes = new List<WriteModel<BsonDocument>>()
        {
            inserts,
            updates
        };
 
        var books = client.GetDatabase("appdb").GetCollection<BsonDocument>("books");
        var bulkWritesResult = books.BulkWrite(writes, new BulkWriteOptions(){IsOrdered = false});
        Console.WriteLine($"deleted = {bulkWritesResult.DeletedCount}");
        Console.WriteLine($"modified= {bulkWritesResult.ModifiedCount}");
        Console.WriteLine($"inserted = {bulkWritesResult.InsertedCount}");
    }
}