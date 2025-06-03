using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions options, IConfiguration configuration) : base(options)
		{
			ChangeTracker.LazyLoadingEnabled = true;
		}

		public DbSet<UserShard> UserShards { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Assignment> Assignments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		public override int SaveChanges()
		{
			return base.SaveChanges();
		}
	}
}

