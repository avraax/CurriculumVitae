using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories.Interfaces;

public interface IEducationRepository
{
    Task<IEnumerable<Education>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Education?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
} 