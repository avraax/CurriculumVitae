using Microsoft.Data.Sqlite;
using FluentAssertions;
using Xunit;
using CurriculumVitae.Models;
using CurriculumVitae.Repositories;

namespace CurriculumVitae.Tests.Repositories;

public class CompanyRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly CompanyRepository _repository;
    private readonly string _tempDbPath;

    public CompanyRepositoryTests()
    {
        _tempDbPath = Path.Combine(Path.GetTempPath(), $"test_cv_{Guid.NewGuid()}.db");
        var connectionString = $"Data Source={_tempDbPath}";
        
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
        
        CreateTestSchema();
        SeedTestData();
        
        _repository = new CompanyRepository(connectionString);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCompanies_OrderedByName()
    {
        var result = await _repository.GetAllAsync();
        
        result.Should().NotBeEmpty();
        result.Should().HaveCount(4);
        
        var resultList = result.ToList();
        resultList[0].Name.Should().Be("Digital Innovations Inc");
        resultList[1].Name.Should().Be("Microsoft Corporation");
        resultList[2].Name.Should().Be("OpenAI");
        resultList[3].Name.Should().Be("TechCorp Solutions");
    }

    private void CreateTestSchema()
    {
        var createTableSql = @"
            CREATE TABLE IF NOT EXISTS Company (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT,
                Industry TEXT,
                Location TEXT,
                Website TEXT,
                EmployeeCount INTEGER,
                FoundedDate TEXT,
                Notes TEXT
            )";
            
        using var command = _connection.CreateCommand();
        command.CommandText = createTableSql;
        command.ExecuteNonQuery();
    }

    private void SeedTestData()
    {
        var companies = new[]
        {
            new { Name = "TechCorp Solutions", Description = "Leading technology solutions provider specializing in enterprise software development and cloud infrastructure.", Industry = "Technology", Location = "San Francisco, CA", Website = "https://techcorp-solutions.com", EmployeeCount = 2500, FoundedDate = new DateTime(2010, 3, 15), Notes = "Publicly traded company (NASDAQ: TCHS). Known for innovative microservices architecture solutions." },
            new { Name = "Digital Innovations Inc", Description = "Digital transformation consultancy helping businesses modernize their technology stack and processes.", Industry = "Consulting", Location = "New York, NY", Website = "https://digitalinnovations.com", EmployeeCount = 850, FoundedDate = new DateTime(2015, 7, 22), Notes = "Privately held. Specializes in React and .NET development." },
            new { Name = "Microsoft Corporation", Description = "Multinational technology corporation producing computer software, consumer electronics, and personal computers.", Industry = "Technology", Location = "Redmond, WA", Website = "https://microsoft.com", EmployeeCount = 221000, FoundedDate = new DateTime(1975, 4, 4), Notes = "Fortune 500 company. Major cloud computing provider with Azure platform." },
            new { Name = "OpenAI", Description = "AI research and deployment company focused on ensuring artificial general intelligence benefits all of humanity.", Industry = "Artificial Intelligence", Location = "San Francisco, CA", Website = "https://openai.com", EmployeeCount = 500, FoundedDate = new DateTime(2015, 12, 11), Notes = "Creator of ChatGPT and GPT models. Leading research in artificial intelligence." }
        };

        foreach (var company in companies)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Company (Name, Description, Industry, Location, Website, EmployeeCount, FoundedDate, Notes) 
                VALUES (@Name, @Description, @Industry, @Location, @Website, @EmployeeCount, @FoundedDate, @Notes)";
            
            command.Parameters.AddWithValue("@Name", company.Name);
            command.Parameters.AddWithValue("@Description", company.Description);
            command.Parameters.AddWithValue("@Industry", company.Industry);
            command.Parameters.AddWithValue("@Location", company.Location);
            command.Parameters.AddWithValue("@Website", company.Website);
            command.Parameters.AddWithValue("@EmployeeCount", company.EmployeeCount);
            command.Parameters.AddWithValue("@FoundedDate", company.FoundedDate);
            command.Parameters.AddWithValue("@Notes", company.Notes);
            
            command.ExecuteNonQuery();
        }
    }

    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
        
        // Cleanup test database file
        if (File.Exists(_tempDbPath))
        {
            try
            {
                File.Delete(_tempDbPath);
            }
            catch
            {
                // Ignore cleanup errors in tests
            }
        }
    }
} 