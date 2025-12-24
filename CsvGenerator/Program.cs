using Bogus;
using CsvHelper;
using System.Globalization;

// Standalone CSV Generator Script
// This script generates a CSV file with 10 rows of dummy test data
// Columns: Name, Age, Email, Phone

Console.WriteLine("=== Test Data CSV Generator ===\n");

// Define the data structure
var personFaker = new Faker<PersonRecord>("en_GB")
    .CustomInstantiator(f => new PersonRecord(
        Name: f.Name.FullName(),
        Age: f.Random.Int(18, 80),
        Email: f.Internet.Email(),
        Phone: f.Phone.PhoneNumber("07### ######")
    ));

// Generate 10 rows of dummy data
var testData = personFaker.Generate(10);

// Create output file path
var outputPath = "dummy_test_data.csv";

// Write to CSV
using (var writer = new StreamWriter(outputPath))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(testData);
}

Console.WriteLine($"CSV file created: {outputPath}");
Console.WriteLine($"Rows generated: {testData.Count}\n");

// Display the generated data
Console.WriteLine("Generated Data:");
Console.WriteLine(new string('-', 80));
Console.WriteLine($"{"Name",-25} {"Age",-5} {"Email",-30} {"Phone",-15}");
Console.WriteLine(new string('-', 80));

foreach (var person in testData)
{
    Console.WriteLine($"{person.Name,-25} {person.Age,-5} {person.Email,-30} {person.Phone,-15}");
}

Console.WriteLine(new string('-', 80));
Console.WriteLine($"\nFile saved to: {Path.GetFullPath(outputPath)}");

// Record type for CSV data
public record PersonRecord(string Name, int Age, string Email, string Phone);
