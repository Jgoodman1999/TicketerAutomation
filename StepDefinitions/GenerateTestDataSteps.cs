using CsvHelper;
using FluentAssertions;
using Reqnroll;
using System.Globalization;
using NUnit.Framework;
using TicketerAutomation.Utilities;

namespace TicketerAutomation.StepDefinitions;

[Binding]
public class GenerateTestDataSteps
{
    private readonly ScenarioContext _scenarioContext;
    private string _csvFilePath = string.Empty;
    private List<TestDataGenerator.PersonData>? _generatedData;

    public GenerateTestDataSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"I want to create a test data CSV file")]
    public void GivenIWantToCreateATestDataCsvFile()
    {
        var testOutputDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestData");
        if (!Directory.Exists(testOutputDir))
        {
            Directory.CreateDirectory(testOutputDir);
        }
        _csvFilePath = Path.Combine(testOutputDir, $"dummy_data_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        _scenarioContext.Set(_csvFilePath, "CsvFilePath");
    }

    [When(@"I generate the CSV file with (.*) rows")]
    public void WhenIGenerateTheCsvFileWithRows(int rowCount)
    {
        var generator = new TestDataGenerator();
        _generatedData = generator.GeneratePersonData(rowCount);
        
        using var writer = new StreamWriter(_csvFilePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(_generatedData);
        
        _scenarioContext.Set(_generatedData, "GeneratedData");
    }

    [Then(@"the CSV file should be created successfully")]
    public void ThenTheCsvFileShouldBeCreatedSuccessfully()
    {
        File.Exists(_csvFilePath).Should().BeTrue($"CSV file should exist at {_csvFilePath}");
        TestContext.WriteLine($"CSV file created at: {_csvFilePath}");
    }

    [Then(@"the CSV file should contain (.*) columns: (.*)")]
    public void ThenTheCsvFileShouldContainColumns(int columnCount, string columnNames)
    {
        using var reader = new StreamReader(_csvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord;
        
        headers.Should().NotBeNull();
        headers!.Length.Should().Be(columnCount, $"CSV should have {columnCount} columns");
        
        var expectedColumns = columnNames.Split(',').Select(c => c.Trim()).ToArray();
        foreach (var column in expectedColumns)
        {
            headers.Should().Contain(column, $"CSV should contain column '{column}'");
        }
        
        TestContext.WriteLine($"CSV columns: {string.Join(", ", headers)}");
    }

    [Then(@"the CSV file should contain (.*) rows of data")]
    public void ThenTheCsvFileShouldContainRowsOfData(int expectedRowCount)
    {
        using var reader = new StreamReader(_csvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<TestDataGenerator.PersonData>().ToList();
        records.Count.Should().Be(expectedRowCount, $"CSV should contain {expectedRowCount} rows");
        
        TestContext.WriteLine($"CSV contains {records.Count} rows of data");
    }

    [Then(@"each row should contain different randomly generated data")]
    public void ThenEachRowShouldContainDifferentRandomlyGeneratedData()
    {
        using var reader = new StreamReader(_csvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<TestDataGenerator.PersonData>().ToList();
        
        var uniqueNames = records.Select(r => r.Name).Distinct().Count();
        uniqueNames.Should().Be(records.Count, "All names should be unique");
        
        var uniqueEmails = records.Select(r => r.Email).Distinct().Count();
        uniqueEmails.Should().Be(records.Count, "All emails should be unique");
        
        foreach (var record in records)
        {
            record.Name.Should().NotBeNullOrWhiteSpace("Name should not be empty");
            record.Age.Should().BeInRange(18, 80, "Age should be between 18 and 80");
            record.Email.Should().Contain("@", "Email should be valid format");
            record.Phone.Should().NotBeNullOrWhiteSpace("Phone should not be empty");
        }
        
        TestContext.WriteLine("All rows contain unique, randomly generated data");
        
        TestContext.WriteLine("\nSample generated data:");
        foreach (var record in records.Take(3))
        {
            TestContext.WriteLine($"  Name: {record.Name}, Age: {record.Age}, Email: {record.Email}, Phone: {record.Phone}");
        }
    }
}
