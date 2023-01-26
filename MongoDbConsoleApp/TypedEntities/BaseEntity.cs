using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MongoDBConsoleApp.Repository;

public abstract class BaseEntity
{
    public ObjectId Id { get; set; }
}