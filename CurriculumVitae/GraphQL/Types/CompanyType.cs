using CurriculumVitae.Models;
using GraphQL.Types;

namespace CurriculumVitae.GraphQL.Types;

public class CompanyType : ObjectGraphType<Company>
{
    public CompanyType()
    {
        Name = "Company";
        Description = "Represents a company or organization";

        Field(c => c.Id)
            .Description("Unique identifier for the company");

        Field(c => c.Name)
            .Description("Name of the company");

        Field(c => c.Description, nullable: true)
            .Description("Description of the company");

        Field(c => c.Industry, nullable: true)
            .Description("Industry sector of the company");

        Field(c => c.Location, nullable: true)
            .Description("Location of the company");

        Field(c => c.Website, nullable: true)
            .Description("Company website URL");

        Field(c => c.EmployeeCount, nullable: true)
            .Description("Number of employees");

        Field(c => c.FoundedDate, nullable: true)
            .Description("Date when the company was founded");

        Field(c => c.Notes, nullable: true)
            .Description("Additional notes about the company");
    }
} 