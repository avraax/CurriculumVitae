using CurriculumVitae.Models;
using GraphQL.Types;

namespace CurriculumVitae.GraphQL.Types;

public class ProjectType : ObjectGraphType<Project>
{
    public ProjectType()
    {
        Name = "Project";
        Description = "Represents a project in a person's CV";

        Field(p => p.Id, type: typeof(NonNullGraphType<IntGraphType>))
            .Description("The unique identifier for the project");
            
        Field(p => p.PersonId, type: typeof(NonNullGraphType<IntGraphType>))
            .Description("The ID of the person this project belongs to");
            
        Field(p => p.Name, type: typeof(NonNullGraphType<StringGraphType>))
            .Description("The name of the project");
            
        Field(p => p.Description, type: typeof(NonNullGraphType<StringGraphType>))
            .Description("A detailed description of the project");
            
        Field(p => p.Technologies, nullable: true)
            .Description("The technologies and tools used in the project");
            
        Field(p => p.StartDate, type: typeof(NonNullGraphType<DateTimeGraphType>))
            .Description("The start date of the project");
            
        Field(p => p.EndDate, nullable: true)
            .Description("The end date of the project. Null indicates project is ongoing");
            
        Field(p => p.ProjectUrl, nullable: true)
            .Description("The URL to the project (GitHub, live demo, etc.)");
            
        Field(p => p.SourceCodeUrl, nullable: true)
            .Description("The URL to the project's source code repository");
            
        Field(p => p.IsFeatured, type: typeof(NonNullGraphType<BooleanGraphType>))
            .Description("Indicates whether this project should be featured prominently");
            
        Field(p => p.IsOngoing, type: typeof(NonNullGraphType<BooleanGraphType>))
            .Description("Indicates whether this project is currently ongoing");
            
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<StringGraphType>>>>("technologiesList")
            .Resolve(context => context.Source.TechnologiesList)
            .Description("The technologies used as a list");
    }
} 