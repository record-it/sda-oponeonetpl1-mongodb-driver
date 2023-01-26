using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDBConsoleApp.Data;
using MongoDBConsoleApp.Filter;
using MongoDbConsoleApp.TypedEntities;
using MongoDBConsoleApp.TypedEntities;

namespace MongoDBConsoleApp
{
    public class Program
    {
        private static List<City>? cities;
        private static List<Country>? countries;
        public static MongoDbConfiguration? Configuration { get; private set; }

        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build()
                .GetSection("Data:MongoDB")
                .Get<MongoDbConfiguration>();
            
            var client = new MongoClient(Configuration.ConnectionURI);
            
            IMongoCollection<Book> books = client.GetDatabase(Configuration.Database)
                .GetCollection<Book>(Configuration.Books);
            var book = books
                .Find(Builders<Book>.Filter.Eq("_id", new ObjectId("63c051e3796e740de326202a")))
                .SingleOrDefault();
            
            long count = TypedCRUDExample.Update(books, new Book() {Id = new ObjectId("63c2b43e9d5f3ead189ac9d8"), Title = "Nowy tytuł", Authors = new List<string>(){"Xmen", "Avengers"}});
            Console.WriteLine("Modified " + count);
        }

        public static void ExerciseTwo(MongoClient client)
        {
            countries = GeoNames.LoadCountries();
            Console.WriteLine(countries.Count);
            var database = client.GetDatabase(Configuration.Database);
            database.DropCollection("countries");
            var countriesCollection = database?.GetCollection<BsonDocument>("countries");
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
            });
            countriesCollection?.InsertMany(enumerable);
        }

        public static void ExerciseOne(MongoClient client)
        {
            cities = GeoNames.LoadCities();
            Console.WriteLine(cities.Count);
            var database = client.GetDatabase(Configuration.Database);
            database.DropCollection("cities");
            var citiesCollection = database?.GetCollection<BsonDocument>("cities");
            if (citiesCollection is not null)
            {
                Console.WriteLine("Cities collection created!");
            }

            var enumerable = cities.Select(city => new BsonDocument()
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
    }
}