using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBConsoleApp.BsonDocumentEntites;

public class BsonDocumentExample
{
    public static void Demo(MongoClient client)
    {
        IMongoCollection<BsonDocument> books = client.GetDatabase(Program.Configuration.Database)
            .GetCollection<BsonDocument>(Program.Configuration.Books);

        /***
         * Inserting new document
         */
        var document = BuildBook("new Title", new[] {"AA BB", "BB BB"}, DateTime.Now, false, 12.4m);
        InsertDocument(books, document);

        /***
         * Finding all documents
         */
        var list = FindAllAndSortBy(books, "_id", false);
        list = FindByPaged(
            books,
            Builders<BsonDocument>.Filter.StringIn("title", "Java", "C#"),
            1,
            4
        );
        /***
         * Deleting book by id
         */
        bool result = DeleteById(books, "63c2b46c405f16da133944c5").Result;
        Console.WriteLine($"Deleted {result}");

        /***
         * Update one property of document
         */
        result = UpdateStringProperty(books, "63bf40db7754335d1ea3a79b", "title", "Effective Java").Result;
        Console.WriteLine($"Updated {result}");

        /***
         * Enumerating  document's collection
         */
        foreach (var book in FindAllAndSortBy(books, "id", true))
        {
            Console.WriteLine(book["_id"] + " " + book["authors"] + " " + book["title"]);
        }
    }

    /// <summary>
    /// Creates and return book document
    /// </summary>
    /// <param name="title">book title</param>
    /// <param name="authors">authors table</param>
    /// <param name="date">creation date</param>
    /// <returns></returns>
    public static BsonDocument BuildBook(string title, string[] authors, DateTime date, bool isAvailable, decimal size)
    {
        return new BsonDocument()
        {
            {"title", title},
            {"authors", new BsonArray(authors.Select(a => new BsonString(a)).ToList())},
            {"created", new BsonDateTime(date)},
            {"is_available", new BsonBoolean(isAvailable)},
            {"ebook_size", new BsonDecimal128(size)},
            {
                "publisher", new BsonDocument()
                {
                    {"name", "Helion"},
                    {"email", "contact@helion.pl"}
                }
            }
        };
    }
    public static BsonDocument BuildBook(Dictionary<string, Object> fields)
    {
        return new BsonDocument().AddRange(fields);
    }

    /// <summary>
    /// Add book document to collection
    /// </summary>
    /// <param name="collection">kolekcja</param>
    /// <param name="document">dodawany dokument</param>
    public static void InsertDocument(IMongoCollection<BsonDocument> collection, BsonDocument document)
    {
        collection.InsertOne(document);
    }

    /***
     * 
     */
    /// <summary>
    /// Get collection of all documents ordered by property
    /// </summary>
    /// <param name="collection">collection</param>
    /// <param name="property">property</param>
    /// <param name="ascending"><code>true</code>ascending,<code>false</code>descending</param>
    /// <returns></returns>
    public static List<BsonDocument> FindAllAndSortBy(IMongoCollection<BsonDocument> collection, string property,
        bool ascending)
    {
        var sort = ascending
            ? Builders<BsonDocument>.Sort.Ascending(property)
            : Builders<BsonDocument>.Sort.Descending(property);
        var cursor = collection.Find(_ => true).Sort(sort);
        return cursor.ToList();
    }

    /// <summary>
    /// Finds document by id
    /// </summary>
    /// <param name="collection">document collection</param>
    /// <param name="id">document id </param>
    /// <returns></returns>
    public static async Task<BsonDocument> FindById(IMongoCollection<BsonDocument> collection, string id)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
        IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
        BsonDocument document = await cursor.SingleOrDefaultAsync();
        return document;
    }

    /// <summary>
    /// Finds paged document.
    /// </summary>
    /// <param name="collection">kolekcja dokumentów</param>
    /// <param name="filter">filtr</param>
    /// <param name="page">numer strony</param>
    /// <param name="size">liczba dokumentów na stronie</param>
    /// <returns></returns>
    public static List<BsonDocument> FindByPaged(IMongoCollection<BsonDocument> collection,
        FilterDefinition<BsonDocument> filter, int page, int size)
    {
        //Można włączać pola
        var projection1 = Builders<BsonDocument>
            .Projection
            .Include("title")
            .Include("authors");
        //Można też wyłączać pola, które należy pominąć
        var projection2 = Builders<BsonDocument>
            .Projection
            .Exclude("created");
        return collection.Find(filter).Project(projection2).Skip(page - 1).Limit(size).ToList();
    }

    /// <summary>
    /// Deletes document by id.
    /// </summary>
    /// <param name="collection">document collection</param>
    /// <param name="id">document id</param>
    /// <returns></returns>
    public async static Task<bool> DeleteById(IMongoCollection<BsonDocument> collection, string id)
    {
        var task = await collection.DeleteOneAsync(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id)));
        return task.DeletedCount == 1;
    }

    /// <summary>
    /// Changes string document property. 
    /// </summary>
    /// <param name="collection">document collection</param> 
    /// <param name="id">docuemnt id</param> 
    /// <param name="property">property</param> 
    /// <param name="value">new property value</param> 
    /// <returns><code>true</code>if update successes</returns>
    public static async Task<bool> UpdateStringProperty(IMongoCollection<BsonDocument> collection, string id,
        string property, string value)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
        var update = Builders<BsonDocument>.Update.Set(property, value);
        var result = await collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount == 1;
    }

    /// <summary>
    /// Zwrócenie kolekcji dokumentów z okresleniem parametru batchSize. Parametr ten określa
    /// maksymalną liczbę dokumentów umieszczaną w jednym cyklu komunikacji sterownika z serwerem.
    /// Domyślna wartość wynosi 100, zmniejszenie do 1000 nie powinno znacząco wpływać na wydajność.
    /// Małe wartości (10..100) mogą znacząco zwiększać czas oczekiwania na wykonanie operacji. 
    /// </summary>
    /// <param name="collection">document collection</param>
    /// <param name="batchSize">batch size<code>batchSize</code></param>
    /// <returns></returns>
    public static async Task<List<BsonDocument>> FindWithOption(IMongoCollection<BsonDocument> collection,
        int batchSize)
    {
        var options = new FindOptions<BsonDocument>()
        {
            BatchSize = batchSize
        };
        var cursor = await collection.FindAsync(_ => true, options);
        return cursor.ToList();
    }
}