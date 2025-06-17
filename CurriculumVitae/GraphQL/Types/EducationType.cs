using CurriculumVitae.Models;
using GraphQL.Types;

namespace CurriculumVitae.GraphQL.Types;

public class EducationType : ObjectGraphType<Education>
{
    public EducationType()
    {
        Name = "Education";
        Description = "Represents an education record in a person's CV";

        Field(e => e.Id, type: typeof(NonNullGraphType<IntGraphType>))
            .Description("The unique identifier for the education record");
            
        Field(e => e.PersonId, type: typeof(NonNullGraphType<IntGraphType>))
            .Description("The ID of the person this education belongs to");
            
        Field(e => e.Institution, type: typeof(NonNullGraphType<StringGraphType>))
            .Description("The name of the educational institution");
            
        Field(e => e.Degree, type: typeof(NonNullGraphType<StringGraphType>))
            .Description("The degree or qualification obtained");
            
        Field(e => e.FieldOfStudy, nullable: true)
            .Description("The field of study or major");
            
        Field(e => e.StartDate, type: typeof(NonNullGraphType<DateTimeGraphType>))
            .Description("The start date of the education period");
            
        Field(e => e.EndDate, nullable: true)
            .Description("The end date of the education period. Null indicates currently studying");
            
        Field(e => e.Grade, nullable: true)
            .Description("The GPA or grade achieved");
            
        Field(e => e.Description, nullable: true)
            .Description("Additional description or achievements");
            
        Field(e => e.IsInProgress, type: typeof(NonNullGraphType<BooleanGraphType>))
            .Description("Indicates whether this education is currently in progress");
    }
} 