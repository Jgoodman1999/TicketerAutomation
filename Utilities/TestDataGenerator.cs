using Bogus;
using CsvHelper;
using System.Globalization;

namespace TicketerAutomation.Utilities;

public class TestDataGenerator
{
    private readonly Faker _faker;

    public TestDataGenerator()
    {
        _faker = new Faker("en_GB");
    }

    public record PersonData(string Name, int Age, string Email, string Phone);

    public List<PersonData> GeneratePersonData(int count = 10)
    {
        var personFaker = new Faker<PersonData>("en_GB")
            .CustomInstantiator(f => new PersonData(
                Name: f.Name.FullName(),
                Age: f.Random.Int(18, 80),
                Email: f.Internet.Email(),
                Phone: f.Phone.PhoneNumber("07### ######")
            ));

        return personFaker.Generate(count);
    }

    public string GenerateCsvFile(string filePath, int rowCount = 10)
    {
        var data = GeneratePersonData(rowCount);
        
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        csv.WriteRecords(data);
        
        Console.WriteLine($"Generated CSV file with {rowCount} rows at: {filePath}");
        return filePath;
    }

    public static void GenerateAndSaveTestData(string outputPath = "TestData/dummy_data.csv")
    {
        var directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var generator = new TestDataGenerator();
        generator.GenerateCsvFile(outputPath);
    }
}
