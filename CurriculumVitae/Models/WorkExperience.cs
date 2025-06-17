using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurriculumVitae.Models;

public class WorkExperience
{
    public int Id { get; set; }

    [Required]
    public int PersonId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Company { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Position { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person Person { get; set; } = null!;

    public bool IsCurrent => EndDate == null;
} 