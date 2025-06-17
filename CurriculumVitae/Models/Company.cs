using System.ComponentModel.DataAnnotations;

namespace CurriculumVitae.Models;

public class Company
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string? Industry { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    [MaxLength(200)]
    public string? Website { get; set; }

    public int? EmployeeCount { get; set; }

    public DateTime? FoundedDate { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
} 