using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoWebAPIApp.Data;

public class MongoDbService
{
    private readonly IMongoCollection<Book> _books;
    private readonly MongoClient _client;

    public MongoDbService(IOptions<MongoDbSettings> mongoDBSettings)
    {
        _client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = _client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _books = database.GetCollection<Book>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<string> GetTitle(string id)
    {
        IMongoCollection<BsonDocument> collection = _client.GetDatabase("appdb").GetCollection<BsonDocument>("books");
        var cursor = await collection.FindAsync(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id)));
        var doc = await cursor.SingleOrDefaultAsync();
        return doc["title"].AsString;
    }

    public async Task<List<Book>> GetAsync()
    {
        return await _books.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<Book> FindById(string id)
    {
        FilterDefinition<Book> filter = Builders<Book>.Filter.Eq(b => b.Id, id);
        IAsyncCursor<Book> cursor = await _books.FindAsync(filter);
        return await _books.FindAsync(filter).Result.SingleOrDefaultAsync();
    }

    public async Task CreateAsync(Book book)
    {
        await _books.InsertOneAsync(book);
    }

    public async Task AddAuthorAsync(string id, string author)
    {
        FilterDefinition<Book> filter = Builders<Book>.Filter.Eq("Id", id);
        UpdateDefinition<Book> update = Builders<Book>.Update.AddToSet<string>("movieIds", id);
        await _books.UpdateOneAsync(filter, update);
    }

    public async Task DeleteAsync(string id)
    {
        await _books.FindOneAndDeleteAsync(Builders<Book>.Filter.Eq(b => b.Id, id));
    }

    public async Task Update(string id, Book book)
    {
        await _books.FindOneAndUpdateAsync(
            Builders<Book>.Filter.Eq(b => b.Id, id),
            Builders<Book>.Update.Combine(
                Builders<Book>.Update.Set(b => b.Title, book.Title),
                Builders<Book>.Update.Set(b => b.Authors, book.Authors)
            ));
    }
}