using Microsoft.Data.Sqlite;
using CurriculumVitae.Repositories.Interfaces;
using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly string _connectionString;

    public ProjectRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Project ORDER BY IsFeatured DESC, StartDate DESC";
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var projects = new List<Project>();
        
        while (await reader.ReadAsync(cancellationToken))
        {
            projects.Add(MapFromReader(reader));
        }
        
        return projects;
    }

    public async Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Project WHERE Id = @Id";
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

    private static Project MapFromReader(SqliteDataReader reader)
    {
        return new Project
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            Technologies = reader.IsDBNull(3) ? null : reader.GetString(3),
            StartDate = DateTime.Parse(reader.GetString(4)),
            EndDate = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
            ProjectUrl = reader.IsDBNull(6) ? null : reader.GetString(6),
            SourceCodeUrl = reader.IsDBNull(7) ? null : reader.GetString(7),
            IsFeatured = reader.GetInt32(8) == 1
        };
    }
} 