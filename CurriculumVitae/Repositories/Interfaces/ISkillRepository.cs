using CurriculumVitae.Models;

namespace CurriculumVitae.Repositories.Interfaces;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Skill?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
} 