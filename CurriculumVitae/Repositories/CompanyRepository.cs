using Microsoft.Data.Sqlite;
using CurriculumVitae.Repositories.Interfaces;
using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly string _connectionString;

    public CompanyRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Company>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Company ORDER BY Name";
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var companies = new List<Company>();
        
        while (await reader.ReadAsync(cancellationToken))
        {
            companies.Add(MapFromReader(reader));
        }
        
        return companies;
    }

    public async Task<Company?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Company WHERE Id = @Id";
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

    private static Company MapFromReader(SqliteDataReader reader)
    {
        return new Company
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
            Industry = reader.IsDBNull(3) ? null : reader.GetString(3),
            Location = reader.IsDBNull(4) ? null : reader.GetString(4),
            Website = reader.IsDBNull(5) ? null : reader.GetString(5),
            EmployeeCount = reader.IsDBNull(6) ? null : reader.GetInt32(6),
            FoundedDate = reader.IsDBNull(7) ? null : DateTime.Parse(reader.GetString(7)),
            Notes = reader.IsDBNull(8) ? null : reader.GetString(8)
        };
    }
} 