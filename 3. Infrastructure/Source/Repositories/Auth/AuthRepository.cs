using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
  public class AuthRepository(IConfiguration configuration) : IAuth
  {
    async public Task<LoginResponse> Login(LoginRequest request)
    {
      int userShard = 0;
      using (var defaultDbContext = AppDBContextFactory.CreateDbContext(0, configuration))
      {
        try
        {
          var user = await defaultDbContext.UserShards
            .Where(e => e.Username == request.Credential || e.Email == request.Credential)
            .Where(e => !e.IsArchived)
            .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found!");

          userShard = user.Shard;
        }
        catch
        {
          throw;
        }
      }

      using var appDBContext = AppDBContextFactory.CreateDbContext(userShard, configuration);
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        var user = await appDBContext.Users
          .Where(e => e.Username == request.Credential || e.Email == request.Credential)
          .Where(e => !e.IsArchived)
          .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found!");

        string token = JsonWebTokens.NewJsonWebTokens(user, userShard, configuration);

        return new LoginResponse(HttpStatusCode.OK, "Registered successfully", token);
      }
      catch
      {
        throw;
      }
    }

    async public Task<RegistrationResponse> Register(RegistrationRequest request)
    {
      var userId = Guid.NewGuid();
      var index = AppDBContextFactory.GetShardIndex(userId, configuration); ;

      using (var defaultDbContext = AppDBContextFactory.CreateDbContext(0, configuration))
      {
        using var defaultTransaction = defaultDbContext.Database.BeginTransaction();
        try
        {
          var isEmailExist = await defaultDbContext.UserShards.FirstOrDefaultAsync(u => u.Email == request!.Email);
          if (isEmailExist != null) throw new BadRequestException("Email is already registered!");
          var isUsernameExist = await defaultDbContext.UserShards.FirstOrDefaultAsync(u => u.Username == request!.Username);
          if (isUsernameExist != null) throw new BadRequestException("Username is already registered!");

          defaultDbContext.Add(new UserShard
          {
            Id = userId,
            Shard = index,
            Email = request!.Email!,
            Username = request!.Username!
          });

          defaultDbContext.SaveChanges();
          defaultTransaction.Commit();
        }
        catch
        {
          throw;
        }
      }

      using var appDBContext = AppDBContextFactory.CreateDbContext(index, configuration);
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        var user = new User
        {
          Id = userId,
          FirstName = request!.FirstName,
          LastName = request!.LastName,
          Username = request!.Username,
          Email = request!.Email,
          Password = BCrypt.Net.BCrypt.HashPassword(request!.Password),
        };

        appDBContext.Add(user);
        appDBContext.SaveChanges();
        transaction.Commit();

        return new RegistrationResponse(HttpStatusCode.Created, "Registered successfully", user);
      }
      catch
      {
        throw;
      }
    }
  }
}