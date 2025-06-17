using CurriculumVitae.Models;
using GraphQL.Types;

namespace CurriculumVitae.GraphQL.Types;

public class WorkExperienceType : ObjectGraphType<WorkExperience>
{
    public WorkExperienceType()
    {
        Name = "WorkExperience";
        Description = "Represents a work experience entry in a person's CV";

        Field(we => we.Id)
            .Description("Unique identifier for the work experience");

        Field(we => we.PersonId)
            .Description("ID of the person this work experience belongs to");

        Field(we => we.Company)
            .Description("Name of the company or organization");

        Field(we => we.Position)
            .Description("Job title or position held");

        Field(we => we.StartDate)
            .Description("Start date of employment");

        Field(we => we.EndDate, nullable: true)
            .Description("End date of employment (null indicates current position)");

        Field(we => we.Description, nullable: true)
            .Description("Detailed description of responsibilities and achievements");

        Field(we => we.IsCurrent)
            .Description("Indicates whether this is a current position");
    }
} 