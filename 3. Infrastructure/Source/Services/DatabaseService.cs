using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
	public static class DatabaseService
	{
		public static IServiceCollection DatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			List<IConfigurationSection> shards = [.. configuration.GetSection("Shards").GetChildren()];

			var ShardConfig = new ShardConfiguration();
			var defaultShard = configuration["ConnectionStrings:Default"];

			ShardConfig.DefaultConnection = defaultShard!;

			for (int i = 0; i < shards.Count; i++)
				if (shards[i].Value != null)
					ShardConfig.Shards!.Add(i, shards[i].Value!);	

			// Validate Shards
			if (ShardConfig.Shards == null || ShardConfig.Shards.Count == 0)
			{
				throw new InvalidOperationException("Shards configuration is missing or empty.");
			}

			services.AddSingleton(ShardConfig);
			services.AddScoped<IShardSelector, ShardSelector>();

			services.AddDbContext<AppDBContext>((serviceProvider, options) =>
				{
					var shard = serviceProvider.GetRequiredService<IShardSelector>().GetCurrentShard();
					Console.Write("Shard : ");
					Console.WriteLine(ShardConfig.Shards[shard]);
					options.UseNpgsql(ShardConfig.Shards[shard], b => b.MigrationsAssembly("WebAPI"));
				},
				ServiceLifetime.Scoped
			);

			return services;
		}
	}
}

