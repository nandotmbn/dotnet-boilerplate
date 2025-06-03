using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;
public class UserQuery
{
	[UsePaging]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public IQueryable<User> GetUsers(AppDBContext context) => context.Users;

	[UseProjection]
	public async Task<User?> GetUserAsync(
			Guid id,
			AppDBContext context,
			CancellationToken cancellationToken)
			=> await context.Users.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
}