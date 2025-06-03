using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
  public record ShardConfiguration
  {
    public string? DefaultConnection { get; set; } = string.Empty;
    public Dictionary<int, string>? Shards { get; set; } = [];
  }

  public interface IShardSelector
  {
    int GetCurrentShard();
  }

  public class ShardSelector(IHttpContextAccessor httpContextAccessor) : IShardSelector
  {
    public int GetCurrentShard()
    {
      int index = 0;
      try
      {
        return UserClaim.GetIndex(httpContextAccessor.HttpContext?.User!);
      }
      catch
      {

      }

      return index;
    }
  }

}