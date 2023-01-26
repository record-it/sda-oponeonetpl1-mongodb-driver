using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoWebAPIApp.Data;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }
    
    [BsonElement("title")]
    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;
    
    [BsonElement("authors")]
    [JsonPropertyName("authors")]
    public List<string> Authors { get; init; } = null!;
}