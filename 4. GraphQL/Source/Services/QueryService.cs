
using GraphQL.Source;

namespace GraphQL.Services;

public static class QueryContainer
{
  public static IServiceCollection QueryService(this IServiceCollection services, IConfiguration configuration)
  {
    services
      .AddGraphQLServer()
      .AddQueryType<Query>()
      .AddTypeExtension<UserQuery>()
      // .AddTypeExtension<UserShardQuery>()
      .AddProjections()
      .AddFiltering()
      .AddSorting()
      .AddDbContextCursorPagingProvider();

    return services;
  }
} 