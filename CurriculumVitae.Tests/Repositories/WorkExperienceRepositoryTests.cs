using Microsoft.Data.Sqlite;
using FluentAssertions;
using Xunit;
using CurriculumVitae.Models;
using CurriculumVitae.Repositories;

namespace CurriculumVitae.Tests.Repositories;

public class WorkExperienceRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly WorkExperienceRepository _repository;
    private readonly string _tempDbPath;

    public WorkExperienceRepositoryTests()
    {
        _tempDbPath = Path.Combine(Path.GetTempPath(), $"test_cv_{Guid.NewGuid()}.db");
        var connectionString = $"Data Source={_tempDbPath}";
        
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
        
        CreateTestSchema();
        SeedTestData();
        
        _repository = new WorkExperienceRepository(connectionString);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllWorkExperiences_OrderedByStartDateDesc()
    {
        var result = await _repository.GetAllAsync();
        
        result.Should().NotBeEmpty();
        result.Should().HaveCount(3);
        
        var resultList = result.ToList();
        resultList[0].Company.Should().Be("TechCorp Solutions");
        resultList[1].Company.Should().Be("Another Company"); 
        resultList[2].Company.Should().Be("Digital Innovations Inc");
    }

    private void CreateTestSchema()
    {
        var createTableSql = @"
            CREATE TABLE IF NOT EXISTS WorkExperience (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PersonId INTEGER NOT NULL,
                Company TEXT NOT NULL,
                Position TEXT NOT NULL,
                StartDate TEXT NOT NULL,
                EndDate TEXT,
                Description TEXT
            )";
            
        using var command = _connection.CreateCommand();
        command.CommandText = createTableSql;
        command.ExecuteNonQuery();
    }

    private void SeedTestData()
    {
        var workExperiences = new[]
        {
            new { PersonId = 1, Company = "TechCorp Solutions", Position = "Senior Software Engineer", StartDate = new DateTime(2021, 3, 1), EndDate = (DateTime?)null, Description = "Lead development of microservices architecture." },
            new { PersonId = 1, Company = "Digital Innovations Inc", Position = "Full Stack Developer", StartDate = new DateTime(2019, 1, 15), EndDate = (DateTime?)new DateTime(2021, 2, 28), Description = "Developed web applications using React and ASP.NET Core." },
            new { PersonId = 2, Company = "Another Company", Position = "Developer", StartDate = new DateTime(2020, 1, 1), EndDate = (DateTime?)new DateTime(2022, 1, 1), Description = "Worked on various projects." }
        };

        foreach (var we in workExperiences)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO WorkExperience (PersonId, Company, Position, StartDate, EndDate, Description) 
                VALUES (@PersonId, @Company, @Position, @StartDate, @EndDate, @Description)";
            
            command.Parameters.AddWithValue("@PersonId", we.PersonId);
            command.Parameters.AddWithValue("@Company", we.Company);
            command.Parameters.AddWithValue("@Position", we.Position);
            command.Parameters.AddWithValue("@StartDate", we.StartDate);
            if (we.EndDate.HasValue)
                command.Parameters.AddWithValue("@EndDate", we.EndDate.Value);
            else
                command.Parameters.AddWithValue("@EndDate", DBNull.Value);
            command.Parameters.AddWithValue("@Description", we.Description);
            
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