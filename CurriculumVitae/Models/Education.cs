using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurriculumVitae.Models;

public class Education
{
    public int Id { get; set; }

    [Required]
    public int PersonId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Institution { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Degree { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? FieldOfStudy { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(50)]
    public string? Grade { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [ForeignKey(nameof(PersonId))]
    public Person? Person { get; set; }

    public bool IsInProgress => EndDate == null;
} 