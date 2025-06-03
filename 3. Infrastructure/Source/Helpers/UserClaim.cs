using System.Security.Claims;
using Domain.Errors;

namespace Infrastructure.Helpers
{
  public static class UserClaim
  {
    public static Guid GetId(ClaimsPrincipal claimsPrincipal)
    {
      ClaimsPrincipal currentUser = claimsPrincipal;
      string identifierClaim = currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
      return Guid.TryParse(identifierClaim, out Guid userId)
        ? userId
        : throw new InternalServerErrorException("User Id is not valid when trying to parse!");
    }

    public static int GetIndex(ClaimsPrincipal claimsPrincipal)
    {
      ClaimsPrincipal currentUser = claimsPrincipal;
      string identifierClaim = currentUser.FindFirst(ClaimTypes.Version)!.Value;
      return int.TryParse(identifierClaim, out int index)
        ? index
        : throw new InternalServerErrorException("Index is not valid when trying to parse!");

    }
  }
}

