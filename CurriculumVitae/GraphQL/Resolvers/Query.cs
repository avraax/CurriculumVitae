using CurriculumVitae.Repositories.Interfaces;
using CurriculumVitae.GraphQL.Types;
using GraphQL.Types;
using CurriculumVitae.Models;

namespace CurriculumVitae.GraphQL.Resolvers;

public class QueryType : ObjectGraphType
{
    public QueryType()
    {
        Name = "Query";
        Description = "Root query for CV operations";

        Field<ListGraphType<EducationType>>("educations")
            .Description("Gets all education records")
            .ResolveAsync(async context =>
            {
                var educationRepository = context.RequestServices?.GetService<IEducationRepository>();
                if (educationRepository != null)
                {
                    var educations = await educationRepository.GetAllAsync(context.CancellationToken);
                    return educations.ToArray();
                }
                return new Education[0];
            });

        Field<EducationType>("education")
            .Description("Gets a specific education record by ID")
            .Argument<NonNullGraphType<IntGraphType>>("id", "The education ID")
            .ResolveAsync(async context =>
            {
                var id = context.Arguments?.TryGetValue("id", out var idArg) == true && idArg.Value != null ? (int)idArg.Value : 0;
                var educationRepository = context.RequestServices?.GetService<IEducationRepository>();
                if (educationRepository != null)
                {
                    return await educationRepository.GetByIdAsync(id, context.CancellationToken);
                }
                return null;
            });

        Field<ListGraphType<SkillType>>("skills")
            .Description("Gets all skills")
            .ResolveAsync(async context =>
            {
                var skillRepository = context.RequestServices?.GetService<ISkillRepository>();
                if (skillRepository != null)
                {
                    var skills = await skillRepository.GetAllAsync(context.CancellationToken);
                    return skills.ToArray();
                }
                return new Skill[0];
            });

        Field<SkillType>("skill")
            .Description("Gets a specific skill by ID")
            .Argument<NonNullGraphType<IntGraphType>>("id", "The skill ID")
            .ResolveAsync(async context =>
            {
                var id = context.Arguments?.TryGetValue("id", out var idArg) == true && idArg.Value != null ? (int)idArg.Value : 0;
                var skillRepository = context.RequestServices?.GetService<ISkillRepository>();
                if (skillRepository != null)
                {
                    return await skillRepository.GetByIdAsync(id, context.CancellationToken);
                }
                return null;
            });

        Field<ListGraphType<ProjectType>>("projects")
            .Description("Gets all projects")
            .ResolveAsync(async context =>
            {
                var projectRepository = context.RequestServices?.GetService<IProjectRepository>();
                if (projectRepository != null)
                {
                    var projects = await projectRepository.GetAllAsync(context.CancellationToken);
                    return projects.ToArray();
                }
                return new Project[0];
            });

        Field<ProjectType>("project")
            .Description("Gets a specific project by ID")
            .Argument<NonNullGraphType<IntGraphType>>("id", "The project ID")
            .ResolveAsync(async context =>
            {
                var id = context.Arguments?.TryGetValue("id", out var idArg) == true && idArg.Value != null ? (int)idArg.Value : 0;
                var projectRepository = context.RequestServices?.GetService<IProjectRepository>();
                if (projectRepository != null)
                {
                    return await projectRepository.GetByIdAsync(id, context.CancellationToken);
                }
                return null;
            });

        Field<ListGraphType<CompanyType>>("companies")
            .Description("Gets all companies")
            .ResolveAsync(async context =>
            {
                var companyRepository = context.RequestServices?.GetService<ICompanyRepository>();
                if (companyRepository != null)
                {
                    var companies = await companyRepository.GetAllAsync(context.CancellationToken);
                    return companies.ToArray();
                }
                return new Company[0];
            });

        Field<CompanyType>("company")
            .Description("Gets a specific company by ID")
            .Argument<NonNullGraphType<IntGraphType>>("id", "The company ID")
            .ResolveAsync(async context =>
            {
                var id = context.Arguments?.TryGetValue("id", out var idArg) == true && idArg.Value != null ? (int)idArg.Value : 0;
                var companyRepository = context.RequestServices?.GetService<ICompanyRepository>();
                if (companyRepository != null)
                {
                    return await companyRepository.GetByIdAsync(id, context.CancellationToken);
                }
                return null;
            });
    }
} 