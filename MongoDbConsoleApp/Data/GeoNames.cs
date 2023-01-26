using System.Globalization;
using System.IO.Compression;
using System.Net;

namespace MongoDBConsoleApp.Data;

public record City(
    int Id, 
    string Name, 
    double Latitude, 
    double Longitude, 
    char FeatureClass, 
    string FeaturedCode,
    string CountryCode, 
    long Population,
    int Elevation, 
    string TimeZoneId
    );

public record Country(
    string ISOCode, 
    string ISO3Code, 
    string ISONumeric, 
    string Fips, 
    string CountryName,
    string Capital, 
    double Area, 
    int Population,
    string Continent, 
    string CurrencyCode, 
    string CurrencyName, 
    string Phone, 
    List<string> Neighbours
    );

public class GeoNames
{
    private static readonly string CitiesFileUrl = "http://download.geonames.org/export/dump/cities500.zip";
    private static readonly string CountryFileUrl = "http://download.geonames.org/export/dump/countryInfo.txt";
    private static readonly string PathToZipFile = Path.Combine(Path.GetTempPath(), "cities500.zip");
    private static readonly string CountriesTextFile = "countryInfo.txt";
    private static readonly string CitiesTextFile = "cities500.txt";
    private static readonly string CountriesPathToTextFile = Path.Combine(Path.GetTempPath(), CountriesTextFile);
    private static readonly string CitiesPathToTextFile = Path.Combine(Path.GetTempPath(), CitiesTextFile);
    
    public static List<City> LoadCities()
    {
        WebClient client = new WebClient();
        if (!File.Exists(PathToZipFile))
        {
            Console.WriteLine("Downloading file...");
            client.DownloadFile(CitiesFileUrl, PathToZipFile);
            Console.WriteLine("Extracting file...");
            ZipFile.ExtractToDirectory(PathToZipFile, Path.GetTempPath());
        }
        else
        {
            Console.WriteLine("File already downloaded, opening cached file...");
        }
        
        List<City> cities = new List<City>();
        Console.WriteLine("Starting loading cities ...");
        
        int count = 0;
        string message = "";
        
        CultureInfo specificCulture = CultureInfo.CreateSpecificCulture("en-EN");
        
        foreach (var line in File.ReadLines(CitiesPathToTextFile))
        {
            string[] tokens = line.Split("\t", StringSplitOptions.TrimEntries);
            City city = new City(
                int.Parse(tokens[0]),
                tokens[1],
                double.Parse(tokens[4], NumberStyles.Float, specificCulture),
                double.Parse(tokens[5], NumberStyles.Float, specificCulture),
                tokens[6].ToCharArray()[0],
                tokens[7],
                tokens[8],
                long.Parse(tokens[14]),
                tokens[15].Length > 0 ? int.Parse(tokens[15]) : 0,
                tokens[17]
            );
            cities.Add(city);
            count++;
            if (count % 1000 == 0)
            {
                Console.Write(string.Join("", Enumerable.Repeat("\b", message.Length)));
                message = $"Loading {count} cities";
                Console.Write(message);
            }
        }

        Console.Write(string.Join("", Enumerable.Repeat("\b", message.Length)));
        Console.Write($"Loading {count} cities");
        Console.WriteLine();
        return cities;
    }
    
    public static List<Country> LoadCountries()
    {
        WebClient client = new WebClient();
        if (!File.Exists(CountriesPathToTextFile))
        {
            Console.WriteLine("Downloading file...");
            client.DownloadFile(CountryFileUrl, CountriesPathToTextFile);
        }
        else
        {
            Console.WriteLine("File already downloaded, opening cached file...");
        }
        List<Country> countries = new List<Country>();
        Console.WriteLine("Starting loading countries ...");
        int count = 0;
        string message = "";
        CultureInfo specificCulture = CultureInfo.CreateSpecificCulture("en-EN");
        foreach (var line in File.ReadLines(CountriesPathToTextFile))
        {
            if (line.StartsWith("#"))
            {
                continue;
            }

            string[] tokens = line.Split("\t", StringSplitOptions.TrimEntries);
            Country country = new Country(
                tokens[0],
                tokens[1],
                tokens[2],
                tokens[3],
                tokens[4],
                tokens[5],
                tokens[6].Length != 0 ? double.Parse(tokens[6], NumberStyles.Float, specificCulture) : 0,
                tokens[7].Length != 0 ? int.Parse(tokens[7]) : 0,
                tokens[8],
                tokens[9],
                tokens[10],
                tokens[11],
                new List<string>(tokens[18].Split(", "))
            );
            countries.Add(country);
            count++;
            if (count % 10 == 0)
            {
                Console.Write(string.Join("", Enumerable.Repeat("\b", message.Length)));
                message = $"Loading {count} countries";
                Console.Write(message);
            }
        }
        Console.Write(string.Join("", Enumerable.Repeat("\b", message.Length)));
        Console.Write($"Loading {count} countries");
        Console.WriteLine();
        return countries;
    }
}