using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class UserShardQuery
{
	[UsePaging]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public IQueryable<UserShard> GetUserShards(IConfiguration configuration)
	{
		using var appDBContext = AppDBContextFactory.CreateDbContext(0, configuration);
		var query = appDBContext.UserShards.AsQueryable();
		return query;
	}

	[UseProjection]
	public async Task<UserShard?> GetUserShardAsync(Guid id, IConfiguration configuration, CancellationToken cancellationToken)
	{
		using var appDBContext = AppDBContextFactory.CreateDbContext(0, configuration);
		return await appDBContext.UserShards.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
		
	}
}