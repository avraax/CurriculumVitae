using Microsoft.Data.Sqlite;
using CurriculumVitae.Repositories.Interfaces;
using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly string _connectionString;

    public PersonRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Person?> GetPersonAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = "SELECT * FROM Person LIMIT 1";
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        
        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        
        if (await reader.ReadAsync(cancellationToken))
        {
            return MapFromReader(reader);
        }
        
        return null;
    }

    private static Person MapFromReader(SqliteDataReader reader)
    {
        return new Person
        {
            Id = reader.GetInt32(0),
            FullName = reader.GetString(1),
            Email = reader.GetString(2),
            Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
            Summary = reader.IsDBNull(4) ? null : reader.GetString(4)
        };
    }
} 