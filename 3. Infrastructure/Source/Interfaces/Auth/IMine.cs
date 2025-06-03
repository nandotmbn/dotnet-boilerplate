
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces
{
  public interface IMine
  {
    Task<int> MyShard(Guid userId);
    Task<User> MyProfile();
    Task<UserResponse> MyProfile(Guid userId);
    Task<UserResponse> ChangeMyPassword(ChangePasswordUserRequest request);
  }
}