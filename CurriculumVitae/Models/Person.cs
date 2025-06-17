using System.ComponentModel.DataAnnotations;

namespace CurriculumVitae.Models;

public class Person
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(2000)]
    public string? Summary { get; set; }

    public ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();

    public ICollection<Education> Educations { get; set; } = new List<Education>();

    public ICollection<Project> Projects { get; set; } = new List<Project>();

    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
} 