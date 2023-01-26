using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDBConsoleApp.Repository;

namespace MongoDBConsoleApp.TypedEntities;

public class CityEntity: BaseEntity
{
    [BsonElement("id")]
    public int CityId { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("latitude")]
    public double Latitude { get; set; }
    
    [BsonElement("longitude")]
    [BsonRepresentation(BsonType.Double)]
    public double Longitude { get; set; }
    
    [BsonElement("feature_class")]
    public char FeatureClass { get; set; }
    
    [BsonElement("feature_code")]
    public string FeaturedCode { get; set; }
    
    [BsonElement("country_code")]
    public string CountryCode { get; set; }
    
    [BsonElement("population")]
    public long Population { get; set; }
    
    [BsonElement("elevation")]
    public int Elevation { get; set; }
    
    [BsonElement("time_zone")]
    public string TimeZoneId { get; set; }
    
    
    [BsonIgnore]
    public BsonDocument Country { get; set; }
}