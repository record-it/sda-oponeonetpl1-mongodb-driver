using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbConsoleApp.TypedEntities;

public class Publisher
{
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("email")]
    public string Email { get; set; }
}