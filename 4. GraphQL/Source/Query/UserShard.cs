using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

public class UserShardQuery
{
	[UsePaging]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public IQueryable<UserShard> GetUserShards(AppDBContext context)
	{
		var query = context.UserShards.AsQueryable();
		return query;
	}

	[UseProjection]
	public async Task<UserShard?> GetUserShardAsync(
			Guid id,
			AppDBContext context,
			CancellationToken cancellationToken)
			=> await context.UserShards.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
}