using Microsoft.Data.Sqlite;
using CurriculumVitae.Repositories.Interfaces;
using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories;

public class WorkExperienceRepository : IWorkExperienceRepository
{
    private readonly string _connectionString;

    public WorkExperienceRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<WorkExperience>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM WorkExperience ORDER BY StartDate DESC";
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var workExperiences = new List<WorkExperience>();
        
        while (await reader.ReadAsync(cancellationToken))
        {
            workExperiences.Add(MapFromReader(reader));
        }
        
        return workExperiences;
    }

    public async Task<WorkExperience?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM WorkExperience WHERE Id = @Id";
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Parameters.AddWithValue("@Id", id);
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        
        if (await reader.ReadAsync(cancellationToken))
        {
            return MapFromReader(reader);
        }
        
        return null;
    }

    private static WorkExperience MapFromReader(SqliteDataReader reader)
    {
        return new WorkExperience
        {
            Id = reader.GetInt32(0),
            PersonId = reader.GetInt32(1),
            Company = reader.GetString(2),
            Position = reader.GetString(3),
            StartDate = DateTime.Parse(reader.GetString(4)),
            EndDate = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
            Description = reader.IsDBNull(6) ? null : reader.GetString(6)
        };
    }
} 