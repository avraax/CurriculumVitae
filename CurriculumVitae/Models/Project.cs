using System.ComponentModel.DataAnnotations;

namespace CurriculumVitae.Models;

public class Project
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Technologies { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Url]
    [MaxLength(500)]
    public string? ProjectUrl { get; set; }

    [Url]
    [MaxLength(500)]
    public string? SourceCodeUrl { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsOngoing => EndDate == null;

    public IEnumerable<string> TechnologiesList => 
        Technologies?.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)) ?? Enumerable.Empty<string>();
} 