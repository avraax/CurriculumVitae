using CurriculumVitae.Models;
using GraphQL.Types;

namespace CurriculumVitae.GraphQL.Types;

public class SkillType : ObjectGraphType<Skill>
{
    public SkillType()
    {
        Name = "Skill";
        Description = "Represents a skill or competency in a person's CV";

        Field(s => s.Id, type: typeof(NonNullGraphType<IntGraphType>))
            .Description("The unique identifier for the skill");
            
        Field(s => s.PersonId, type: typeof(NonNullGraphType<IntGraphType>))
            .Description("The ID of the person this skill belongs to");
            
        Field(s => s.Name, type: typeof(NonNullGraphType<StringGraphType>))
            .Description("The name of the skill");
            
        Field(s => s.ProficiencyLevel, type: typeof(NonNullGraphType<IntGraphType>))
            .Description("The proficiency level (1-10 scale)");
            
        Field<NonNullGraphType<SkillCategoryEnumType>>("category")
            .Resolve(context => context.Source.Category)
            .Description("The category this skill belongs to");
            
        Field(s => s.YearsOfExperience, nullable: true)
            .Description("The number of years of experience with this skill");
            
        Field(s => s.Description, nullable: true)
            .Description("Additional description or context for the skill");
    }
}

public class SkillCategoryEnumType : EnumerationGraphType<SkillCategory>
{
    public SkillCategoryEnumType()
    {
        Name = "SkillCategory";
        Description = "Categories for skills to enable organized grouping";
        
        Add("TECHNICAL", SkillCategory.Technical, "Programming languages and frameworks");
        Add("SOFT", SkillCategory.Soft, "Soft skills and interpersonal abilities");
        Add("LANGUAGE", SkillCategory.Language, "Language proficiencies");
        Add("TOOLS", SkillCategory.Tools, "Tools and software applications");
        Add("CERTIFICATION", SkillCategory.Certification, "Certifications and credentials");
    }
} 