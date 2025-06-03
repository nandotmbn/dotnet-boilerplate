
using Domain.Types;

namespace Infrastructure.Interfaces
{
  public interface IAssignment
  {
    Task<AssignmentResponse> GetAssignment(Guid? assignmentId);
    Task<AssignmentsResponse> GetAssignments(string? search, AssignmentProperty? sortProperty, SortType? sortType, int? limit = 10, int? page = 1);
    Task<AssignmentResponse> CreateAssignment(AssignmentRequest request);
    Task<AssignmentsResponse> CreateAssignments(List<AssignmentRequest> requests);
    Task<AssignmentResponse> UpdateAssignment(Guid? assignmentId, AssignmentRequest request);
    Task<AssignmentResponse> ArchiveAssignment(Guid? assignmentId);
    Task<AssignmentsResponse> ArchiveAssignments(List<Guid>? assignmentIds);
  }
}