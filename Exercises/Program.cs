using System.Security.AccessControl;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Exercise;

public class Program
{
    private static string ConnectioString = "mongodb://127.0.0.1:27017";
    private static string Database = "oponeo";
    private static string Cities = "cities";
    private static string Countries = "coutries";
    private static MongoClient Client = new(ConnectioString);

    public static void Main(string[] args)
    {
        Exercise1();
    }

    private static void FindAveragePopulationForEvryCountry()
    {
        GetCitiesEntities().Aggregate()
            .Match(Builders<CityEntity>.Filter.Gte(e => e.Population, 100_000))
            .Group(
                e => e.CountryCode,
                g =>
                    new
                    {
                        g.Key,
                        Count = g.Count(),
                        Population = g.Sum(d => d.Population)
                    }
            )
            .Project(g => new
            {
                CountryCode = g.Key,
                TotalPopulation = g.Population,
                CountryCount = g.Count
            })
            .ToList()
            .ForEach(d =>
            {
                Console.WriteLine($"average population: {(double)d.TotalPopulation/d.CountryCount} for {d.CountryCode}");
            });
    }

    private static void FindPolishCitiesEntity()
    {
        var cities = GetCitiesEntities();
        var filter = Builders<CityEntity>.Filter.Eq(e => e.CountryCode, "PL");
        cities.Find(filter).ToList().ForEach(e => Console.WriteLine(e.Name));
    }
    public static IMongoCollection<CityEntity> GetCitiesEntities() =>
        Client.GetDatabase(Database).GetCollection<CityEntity>(Cities);
    
    private static void FindPolishCities()
    {
        var cities = GetCities();
        var filter = Builders<BsonDocument>.Filter.Eq("country_code", "PL");
        cities
            .Find(filter)
            .SortBy(d => d["name"])
            .ToList()
            .ForEach(d => Console.WriteLine(d["name"]));
    }

    public static IMongoCollection<BsonDocument> GetCities() =>
        Client.GetDatabase(Database).GetCollection<BsonDocument>(Cities);

    public static IMongoCollection<BsonDocument> GetCountries() =>
        Client.GetDatabase(Database).GetCollection<BsonDocument>(Countries);

    public static void Exercise1()
    {
        var countries = GeoNames.LoadCountries();
        Console.WriteLine(countries.Count);
        var database = Client.GetDatabase(Database);
        database.DropCollection(Countries);
        var countriesCollection = database?.GetCollection<BsonDocument>(Countries);
        if (countriesCollection is not null)
        {
            Console.WriteLine("Countries collection created!");
        }

        var enumerable = countries.Select(country => new BsonDocument()
        {
            {"iso_code", country.ISOCode},
            {"iso3_code", country.ISO3Code},
            {"population", country.Population},
            {"name", country.CountryName},
            {"area", country.Area},
            {"capital", country.Capital},
            {"continent", country.Continent},
            {"fips", country.Fips},
            {"phone", country.Phone},
            {"currency_code", country.CurrencyCode},
            {"currency_name", country.CurrencyName},
            {"iso_numeric", country.ISONumeric},
            {"neighbours", new BsonArray(country.Neighbours)}
        });
        countriesCollection?.InsertMany(enumerable);

        var cities = GeoNames.LoadCities();
        Console.WriteLine(cities.Count);
        database = Client.GetDatabase(Database);
        database.DropCollection(Cities);
        var citiesCollection = database?.GetCollection<BsonDocument>(Cities);
        if (citiesCollection is not null)
        {
            Console.WriteLine("Cities collection created!");
        }

        enumerable = cities.Select(city => new BsonDocument()
        {
            {"id", city.Id},
            {"name", city.Name},
            {"population", city.Population},
            {"latitude", city.Latitude},
            {"longitude", city.Longitude},
            {"elevation", city.Elevation},
            {"time_zone", city.TimeZoneId},
            {"country_code", city.CountryCode},
            {"feature_class", city.FeatureClass},
            {"feature_code", city.FeaturedCode}
        });
        citiesCollection?.InsertMany(enumerable);
    }

    public static void Exercise2()
    {
    }

    public static void Exercise3()
    {
    }

    public static void Exercise4()
    {
    }

    public static void Exercise5()
    {
    }

    public static void Exercise6()
    {
    }

    public static void Exercise7()
    {
    }
}