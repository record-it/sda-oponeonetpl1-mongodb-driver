using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDBConsoleApp.BulkOperation;
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
        }

        public static void ExerciseTwo(MongoClient client)
        {
            
        }

        public static void ExerciseOne(MongoClient client)
        {
           
        }
    }
}