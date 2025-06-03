using System.Net;
using Domain.Models;

namespace Domain.Types
{
  public class AssignmentRequest
  {
		public string? Title { get; set; } = string.Empty;
		public string? Description { get; set; } = string.Empty;
  }

  public enum AssignmentProperty
  {
    Title,
    Description,
    CreatedAt,
    UpdatedAt,
    User,
  }

  public record AssignmentResponse(HttpStatusCode StatusCode, string Message, Assignment? Data);
  public record AssignmentsResponse(HttpStatusCode StatusCode, string Message, List<Assignment>? Data, int? Page = 0, int? Limit = 0, int? Total = 0);
}