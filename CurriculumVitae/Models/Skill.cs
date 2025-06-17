using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurriculumVitae.Models;

public class Skill
{
    public int Id { get; set; }

    [Required]
    public int PersonId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 10)]
    public int ProficiencyLevel { get; set; }

    [Required]
    public SkillCategory Category { get; set; }

    [Range(0, 50)]
    public int? YearsOfExperience { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person? Person { get; set; }
}

public enum SkillCategory
{
    Technical = 1,
    Soft = 2,
    Language = 3,
    Tools = 4,
    Certification = 5
} 