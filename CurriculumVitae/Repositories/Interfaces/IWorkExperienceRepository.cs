using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories.Interfaces;

public interface IWorkExperienceRepository
{
    Task<IEnumerable<WorkExperience>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<WorkExperience?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
} 