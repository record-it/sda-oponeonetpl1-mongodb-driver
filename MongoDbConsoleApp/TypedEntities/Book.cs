using MongoDB.Bson.Serialization.Attributes;
using MongoDBConsoleApp.Repository;

namespace MongoDbConsoleApp.TypedEntities;

public class Book: BaseEntity
{
    [BsonElement("title")]
    public string Title { get; set; }
    [BsonElement("authors")]
    public List<string> Authors { get; set; }
    
    [BsonElement("created")]
    public DateTime Created { get; set; }
    [BsonElement("publisher")]
    public Publisher Publisher { get; set; }
}