using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database
{
  public class AppDBContextFactory
  {
    public static AppDBContext CreateDbContext(int shardIndex, IConfiguration configuration)
    {
      string connectionString = configuration[$"Shards:{shardIndex}"]!;
      var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
      optionsBuilder.UseNpgsql(connectionString);
      return new AppDBContext(optionsBuilder.Options, configuration);
    }

    public static int GetShardIndex(Guid id, IConfiguration configuration)
    {
      int shardCount = configuration.GetSection("Shards").GetChildren().Count();
      return (int)(BitConverter.ToUInt32(id.ToByteArray(), 0) % shardCount);
    }
  }
}