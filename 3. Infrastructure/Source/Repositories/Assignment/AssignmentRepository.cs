using Domain.Types;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Domain.Models;
using Infrastructure.Interfaces;
using Domain.Errors;

namespace Infrastructure.Repositories
{
  public class AssignmentRepository(AppDBContext appDBContext, IMine mine) : IAssignment
  {
    public IQueryable<Assignment> Query(string? name)
    {
      var query = appDBContext.Assignments.AsQueryable();
      query = query
        .Where(x => !x.IsArchived);

      if (name != null)
      {
        query = query.Where(x => EF.Functions.Like(x.Title!.ToLower() + " " + x.Description!.ToLower(), $"%{name.ToLower()}%"));
      }
      return query;
    }

    public async Task<Assignment> Create(AssignmentRequest request)
    {
      var me = await mine.MyProfile();
      var assignment = new Assignment
      {
        Title = request.Title,
        Description = request.Description,
        UserId = me.Id
      };

      appDBContext.Add(assignment);

      await appDBContext.SaveChangesAsync();
      return assignment;
    }

    async public Task<Assignment> Archive(Guid? assignmentId)
    {
      var assignment = (await GetAssignment(assignmentId)).Data;

      assignment!.IsArchived = true;
      appDBContext.SaveChanges();

      return assignment;
    }

    async public Task<Assignment> Update(Guid? assignmentId, AssignmentRequest request)
    {
      var assignment = (await GetAssignment(assignmentId)).Data ?? throw new NotFoundException("Assignment is not found!");

      assignment.Title = request.Title;
      assignment.Description = request.Description;
      assignment.UpdatedAt = DateTime.UtcNow;

      appDBContext.SaveChanges();

      return assignment;
    }

    async public Task<AssignmentsResponse> ArchiveAssignments(List<Guid>? assignmentIds)
    {
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        List<Assignment> entities = [];

        foreach (var assignmentId in assignmentIds!)
        {
          var assignmentModel = await Archive(assignmentId);
          entities.Add(assignmentModel);
        }
        transaction.Commit();
        return new AssignmentsResponse(HttpStatusCode.OK, "Assignment is successfully archived", entities);
      }
      catch
      {
        throw;
      }
    }

    async public Task<AssignmentResponse> ArchiveAssignment(Guid? assignmentId)
    {
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        var assignment = await Archive(assignmentId);
        transaction.Commit();
        return new AssignmentResponse(HttpStatusCode.OK, "Assignment is successfully archived", assignment);
      }
      catch
      {
        throw;
      }
    }

    async public Task<AssignmentsResponse> CreateAssignments(List<AssignmentRequest> requests)
    {
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        List<Assignment> assignments = [];
        foreach (var request in requests)
        {
          var assignment = await Create(request);
          assignments.Add(assignment);
        }

        await transaction.CommitAsync();
        return new AssignmentsResponse(HttpStatusCode.Created, "Assignment is successfully created", assignments);
      }
      catch
      {
        throw;
      }
    }

    async public Task<AssignmentResponse> CreateAssignment(AssignmentRequest request)
    {
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        var assignment = await Create(request);
        await transaction.CommitAsync();

        return new AssignmentResponse(HttpStatusCode.Created, "Assignment is successfully created", assignment);
      }
      catch
      {
        throw;
      }
    }

    public async Task<AssignmentsResponse> GetAssignments(string? name, AssignmentProperty? sortProperty, SortType? sortType, int? limit = 10, int? page = 1)
    {
      var me = await mine.MyProfile();    

      int? itemsToSkip = (page - 1) * limit;
      var query = Query(name);

      query = query.Where(x => x.UserId == me.Id);

      if (sortType != null)
      {
        query = sortProperty switch
        {
          AssignmentProperty.Title => sortType == SortType.Asc ? query.OrderBy(x => x.Title) : query.OrderByDescending(x => x.Title),
          AssignmentProperty.Description => sortType == SortType.Asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description),
          AssignmentProperty.CreatedAt => sortType == SortType.Asc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
          AssignmentProperty.UpdatedAt => sortType == SortType.Asc ? query.OrderBy(x => x.UpdatedAt) : query.OrderByDescending(x => x.UpdatedAt),
          _ => sortType == SortType.Asc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
        };
      }
      else
      {
        query = query.OrderByDescending(x => x.CreatedAt);
      }

      var entities = await query.Skip((int)itemsToSkip!).Take((int)limit!).ToListAsync();

      return new AssignmentsResponse(HttpStatusCode.OK, "Assignments are successfully retrieved", entities, page, limit, query.Count());
    }

    public async Task<AssignmentResponse> GetAssignment(Guid? assignmentId)
    {
      var me = await mine.MyProfile();

      var assignment = await appDBContext.Assignments
      .Where(e => !e.IsArchived)
      .Where(e => e.Id == assignmentId)
      .Where(e => e.UserId == me.Id)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Assignment not found");
      return new AssignmentResponse(HttpStatusCode.OK, "Assignment is successfully retrieved", assignment);
    }

    public async Task<AssignmentResponse> UpdateAssignment(Guid? assignmentId, AssignmentRequest request)
    {
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        var assignment = await Update(assignmentId, request);
        transaction.Commit();
        return new AssignmentResponse(HttpStatusCode.OK, "Assignment is successfully updated", assignment);
      }
      catch
      {
        throw;
      }
    }
  }
}