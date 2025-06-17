using Microsoft.Data.Sqlite;
using CurriculumVitae.Repositories.Interfaces;
using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories;

/// <summary>
/// Repository implementation for Education entity using raw SQL statements.
/// </summary>
public class EducationRepository : IEducationRepository
{
    private readonly string _connectionString;

    public EducationRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Education>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Education ORDER BY StartDate DESC";
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var educations = new List<Education>();
        
        while (await reader.ReadAsync(cancellationToken))
        {
            educations.Add(MapFromReader(reader));
        }
        
        return educations;
    }

    /// <summary>
    /// Gets an education record by its ID.
    /// </summary>
    /// <param name="id">The education ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The education record if found, otherwise null.</returns>
    public async Task<Education?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Education WHERE Id = @Id";
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

    private static Education MapFromReader(SqliteDataReader reader)
    {
        return new Education
        {
            Id = reader.GetInt32(0),
            PersonId = reader.GetInt32(1),
            Institution = reader.GetString(2),
            Degree = reader.GetString(3),
            FieldOfStudy = reader.IsDBNull(4) ? null : reader.GetString(4),
            StartDate = DateTime.Parse(reader.GetString(5)),
            EndDate = reader.IsDBNull(6) ? null : DateTime.Parse(reader.GetString(6)),
            Grade = reader.IsDBNull(7) ? null : reader.GetString(7),
            Description = reader.IsDBNull(8) ? null : reader.GetString(8)
        };
    }
} 