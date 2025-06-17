using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories.Interfaces;

public interface IPersonRepository
{
    Task<Person?> GetPersonAsync(CancellationToken cancellationToken = default);
} 