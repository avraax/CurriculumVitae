using Microsoft.Data.Sqlite;
using CurriculumVitae.Repositories.Interfaces;
using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly string _connectionString;

    public SkillRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Skill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Skill ORDER BY ProficiencyLevel DESC, Category ASC";
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var skills = new List<Skill>();
        
        while (await reader.ReadAsync(cancellationToken))
        {
            skills.Add(MapFromReader(reader));
        }
        
        return skills;
    }

    public async Task<Skill?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Skill WHERE Id = @Id";
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

    private static Skill MapFromReader(SqliteDataReader reader)
    {
        return new Skill
        {
            Id = reader.GetInt32(0),
            PersonId = reader.GetInt32(1),
            Name = reader.GetString(2),
            ProficiencyLevel = reader.GetInt32(3),
            Category = (SkillCategory)reader.GetInt32(4),
            YearsOfExperience = reader.IsDBNull(5) ? null : reader.GetInt32(5),
            Description = reader.IsDBNull(6) ? null : reader.GetString(6)
        };
    }
} 