using CurriculumVitae.GraphQL.Resolvers;
using GraphQL.Types;

namespace CurriculumVitae.GraphQL;

public class CurriculumVitaeSchema : Schema
{
    public CurriculumVitaeSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<QueryType>();
    }
} 