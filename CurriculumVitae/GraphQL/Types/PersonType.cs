using CurriculumVitae.Models;
using GraphQL.Types;

namespace CurriculumVitae.GraphQL.Types;

public class PersonType : ObjectGraphType<Person>
{
    public PersonType()
    {
        Name = "Person";
        Description = "Represents a person's CV profile";

        Field(p => p.Id)
            .Description("Unique identifier for the person");

        Field(p => p.FullName)
            .Description("Person's full name");

        Field(p => p.Email, nullable: true)
            .Description("Person's email address");

        Field(p => p.Phone, nullable: true)
            .Description("Person's phone number");

        Field(p => p.Summary, nullable: true)
            .Description("Professional summary or bio");
    }
} 