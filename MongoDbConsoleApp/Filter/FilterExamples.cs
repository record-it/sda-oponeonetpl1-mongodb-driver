using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbConsoleApp.TypedEntities;

namespace MongoDBConsoleApp.Filter;

public class FilterExamples
{
    public static List<BsonDocument> FindBooksWithDigitsInTitle(IMongoCollection<BsonDocument> books)
    {
        /***
         * regular expression example
         */
        var filter = Builders<BsonDocument>.Filter.Regex("title", "/\\d{1,2}/");
        List<BsonDocument> list = books.Find(filter).ToList();
        return list;
    }
    
    public static List<Book> FindBooksInTitleLengthsRange(IMongoCollection<Book> books, int min, int max)
    {
        /***
         * equal operator example
         */
        var filter = Builders<Book>.Filter.And(
            Builders<Book>.Filter.Gte(b=> b.Title.Length, min),
            Builders<Book>.Filter.Lte(b=> b.Title.Length, max)
        );
        return books.Find(filter).ToList();
    }
}