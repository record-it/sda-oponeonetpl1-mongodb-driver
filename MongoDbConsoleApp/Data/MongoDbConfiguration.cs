namespace MongoDBConsoleApp.Data;

public class MongoDbConfiguration
{
    public string ConnectionURI { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string Books { get; set; } = null!;

    public override string ToString()
    {
        return $"{nameof(ConnectionURI)}: {ConnectionURI}, {nameof(Database)}: {Database}, {nameof(Books)}: {Books}";
    }
}