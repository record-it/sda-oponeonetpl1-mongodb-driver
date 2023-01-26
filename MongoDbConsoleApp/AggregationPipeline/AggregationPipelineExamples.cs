using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBConsoleApp.TypedEntities;

namespace MongoDBConsoleApp.AggregtionPipeline;

public class AggregationPipelineExamples
{
    public static long CountCountryPopulation(IMongoCollection<BsonDocument> collection, string countryCode)
    {
        /***
         * Wybrane etapy pipeline w konwencji shell
         * $match       - filtracja, dopasowanie dokumentów 
         * $group       - grupowanie wg wybranych pól, 
         * $sort        - sortowanie
         * $project     - tworzenie projekcji
         * $unwind      - rozpakowanie tablicy i utworznie osobnych obiektów dla każdego elementu
         * $skip        - pominięcie określonej liczby dokumentów
         * $merge       - połączenie dwóch kolekcji
         */

        /***
         * Etap grupowania: $group
         * Funkcje agregujące:
         * $max
         * $min
         * $sum
         * $avg
         */
        var group = new BsonDocument()
        {
            {"_id", "$country_code"},                                       //wskazanie pola grupowania
            {"population", new BsonDocument() {{"$sum", "$population"}}},   //operacja na kolekcji uzyskanej z grupowania
            {"count", new BsonDocument() {{"$sum", 1L}}}
        };

        /***
         * Etap filtrowania: $match
         */
        var match = new BsonDocument()
        {
            {"country_code", countryCode}
        };

        /***
         * Etap projekcji
         */
        var project = new BsonDocument()
        {
            {"_id", 0},
            {"country_code", "$country_code"},
            {"population", "$population"}
        };

        /***
         * Wywołanie etapów za pomocą fluent api
         */
        var population1 = collection.Aggregate()
            .Project(project)
            .Match(match)
            .Group(group)
            .ToCursor().ToList();

        /***
         * Wywołanie etapów zapisanych w tablicy 
         */
        var stages = new[]
        {
            new BsonDocument() {{"$project", project}},
            new BsonDocument() {{"$match", match}},
            new BsonDocument() {{"$group", group}}
        };
        var population2 = collection.Aggregate<BsonDocument>(stages);

        return population2.FirstOrDefault()["population"].AsInt64;
    }

    public static List<(string countryCode, double avg)> CountCountryAvgPopulationFluentApi(
        IMongoCollection<CityEntity> collection)
    {
        var project = collection.Aggregate()
            .Match(Builders<CityEntity>.Filter.Gte(d => d.Population, 5_000))
            .Group(
                d => d.CountryCode, 
                g => new 
                {
                    countryCode = g.Key,
                    population =  g.Sum(k => k.Population),
                    count = g.Count()
                }
            )
            .Project(d => 
            new {
                d.countryCode,
                avg = ((double) d.population )/ d.count
            }).ToCursor().ToList();
        return project.Select(p => (countryCode:  p.countryCode, avg:  p.avg)).ToList();
    }
}