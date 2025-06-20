using Domain.Types;
using Domain.Errors;
using Infrastructure.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Infrastructure.Helpers;

namespace Infrastructure.Repositories
{
  public class MineRepository(AppDBContext appDBContext, IHttpContextAccessor httpContext, IConfiguration configuration) : IMine
  {
    async public Task<UserResponse> ChangeMyPassword(ChangePasswordUserRequest request)
    {
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        var userId = UserClaim.GetId(httpContext.HttpContext!.User);
        var user = await appDBContext.Users.Where(u =>
          u.Id == userId &&
          !u.IsArchived
        ).FirstOrDefaultAsync() ?? throw new NotFoundException("User is not found!");

        bool checkPassword = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user!.Password);
        if (!checkPassword) throw new BadRequestException("Invalid credential!");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        appDBContext.SaveChanges();
        transaction.Commit();
        return new UserResponse(HttpStatusCode.Accepted, "User's password is successfully updated!", user);
      }
      catch
      {
        throw;
      }
    }

    async public Task<UserResponse> MyProfile(Guid userId)
    {
      var mine = await appDBContext.Users.Where(e => e.Id == userId).FirstOrDefaultAsync() ?? throw new NotFoundException("User is not found!");
      return new UserResponse(HttpStatusCode.OK, "Your profile is successfully retrieved", mine);
    }

    async public Task<User> MyProfile()
    {
      Guid myId = UserClaim.GetId(httpContext.HttpContext!.User);
      var mine = await MyProfile(myId);
      return mine.Data!;
    }

    async public Task<int> MyShard(Guid userId)
    {
      int index = 0;
      using (var defaultTransaction = AppDBContextFactory.CreateDbContext(0, configuration))
      {
        var user = await defaultTransaction.UserShards.Where(e => e.Id == userId).FirstOrDefaultAsync() ?? throw new NotFoundException("User is not found!");
        if (user != null)
        {
          index = user.Shard;
        }
      }

      return index;
    }
  }
}