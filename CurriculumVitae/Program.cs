using CurriculumVitae.Data;
using CurriculumVitae.GraphQL;
using CurriculumVitae.GraphQL.Resolvers;
using CurriculumVitae.GraphQL.Types;
using CurriculumVitae.Repositories;
using CurriculumVitae.Repositories.Interfaces;
using GraphQL;
using GraphQL.SystemTextJson;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=CurriculumVitae.db";

builder.Services.AddScoped<IEducationRepository>(provider => 
    new EducationRepository(connectionString));
builder.Services.AddScoped<ISkillRepository>(provider => 
    new SkillRepository(connectionString));
builder.Services.AddScoped<IProjectRepository>(provider => 
    new ProjectRepository(connectionString));
builder.Services.AddScoped<ICompanyRepository>(provider => 
    new CompanyRepository(connectionString));

builder.Services.AddSingleton(new DatabaseInitializer(connectionString));

builder.Services.AddSingleton<EducationType>();
builder.Services.AddSingleton<SkillType>();
builder.Services.AddSingleton<SkillCategoryEnumType>();
builder.Services.AddSingleton<ProjectType>();
builder.Services.AddSingleton<CompanyType>();
builder.Services.AddSingleton<QueryType>();
builder.Services.AddSingleton<CurriculumVitaeSchema>();

builder.Services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
builder.Services.AddSingleton<IGraphQLTextSerializer, GraphQLSerializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await dbInitializer.InitializeAsync();
}

app.UseRouting();

app.MapPost("/graphql", async (HttpContext context, IServiceProvider serviceProvider) =>
{
    var executer = serviceProvider.GetRequiredService<IDocumentExecuter>();
    var schema = serviceProvider.GetRequiredService<CurriculumVitaeSchema>();
    var serializer = serviceProvider.GetRequiredService<IGraphQLTextSerializer>();

    using var reader = new StreamReader(context.Request.Body);
    var requestBody = await reader.ReadToEndAsync();
    var request = JsonConvert.DeserializeObject<GraphQLRequest>(requestBody);
    
    if (request == null || string.IsNullOrEmpty(request.Query))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Invalid request or empty query");
        return;
    }

    var result = await executer.ExecuteAsync(options =>
    {
        options.Schema = schema;
        options.Query = request.Query;
        options.Variables = request.Variables != null ? new Inputs(request.Variables) : null;
        options.OperationName = request.OperationName;
        options.RequestServices = serviceProvider;
    });

    context.Response.ContentType = "application/json";
    var json = serializer.Serialize(result);
    await context.Response.WriteAsync(json);
});



app.Run();

public class GraphQLRequest
{
    public string Query { get; set; } = string.Empty;
    public Dictionary<string, object?>? Variables { get; set; }
    public string? OperationName { get; set; }
} 