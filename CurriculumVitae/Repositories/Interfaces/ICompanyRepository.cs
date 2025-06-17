using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
} 