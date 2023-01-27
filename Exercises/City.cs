using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Exercise;

public class CityEntity
{
    public ObjectId Id { get; set; }
    
    [BsonElement("id")]
    public int CityId { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    [BsonElement("country_code")]
    public string CountryCode { get; set; }
    [BsonElement("population")]
    public long Population { get; set; }
    
    [BsonElement("latitude")]
    public double Latitude { get; set; }
    
    [BsonElement("longitude")]
    public double Longitude { get; set; }
    
    [BsonElement("elevation")]
    public int Elevation { get; set; }
    
    [BsonElement("time_zone")]
    public  string TimeZone { get; set; }
    
    [BsonElement("feature_code")]
    public string FeatureCode { get; set; }
    
    [BsonElement("feature_class")]
    public int FeatureClass { get; set; }
    
}