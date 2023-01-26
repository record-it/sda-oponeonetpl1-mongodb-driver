using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbConsoleApp.TypedEntities;

public class TypedCRUDExample
{
    public static void Insert(IMongoCollection<Book> books, Book book)
    {
        books.InsertOne(book);
    }

    public static bool Delete(IMongoCollection<Book> books, string id)
    {
        var eq = Builders<Book>.Filter.Eq(b => b.Id, new ObjectId(id));
        long count = books.DeleteOne(eq).DeletedCount;
        return count == 1;
    }

    public static long Update(IMongoCollection<Book> books, Book book)
    {
        var eq = Builders<Book>.Filter.Eq(b => b.Id, book.Id);
        UpdateDefinition<Book> definition = Builders<Book>.Update
            .Set(b => b.Authors, book.Authors)
            .Set(b => b.Title, book.Title)
            .Set(b => b.Publisher, book.Publisher);
        UpdateResult result = books.UpdateOne(eq, definition, new UpdateOptions(){Comment = "jam to uczynił"});
        return result.ModifiedCount;
    }

}