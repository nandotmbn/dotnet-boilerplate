using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers
{
  [ApiController]
  [Route("api/assignment")]
  [Authorize]
  public class AssignmentController(IAssignment assignment) : ControllerBase
  {
    [HttpGet]
    public async Task<ActionResult<AssignmentsResponse>> GetAssignments(
      [FromQuery] string? name,
      [FromQuery] AssignmentProperty? sortProperty,
      [FromQuery] SortType? sortType,
      [FromQuery] int limit = 10,
      [FromQuery] int page = 1
    )
    {
      var result = await assignment.GetAssignments(name, sortProperty, sortType, limit, page);
      return Ok(result);
    }

    [HttpGet("assignmentId/{assignmentId}")]
    public async Task<ActionResult<AssignmentResponse>> GetAssignment(Guid assignmentId)
    {
      var result = await assignment.GetAssignment(assignmentId);
      return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AssignmentResponse>> CreateAssignment(AssignmentRequest request)
    {
      var result = await assignment.CreateAssignment(request);
      return Ok(result);
    }

    [HttpPost("bulk")]
    public async Task<ActionResult<AssignmentsResponse>> CreateAssignments(List<AssignmentRequest> requests)
    {
      var result = await assignment.CreateAssignments(requests);
      return Ok(result);
    }

    [HttpPut("assignmentId/{assignmentId}")]
    public async Task<ActionResult<AssignmentResponse>> UpdateAssignment([FromRoute] Guid assignmentId, AssignmentRequest assignmentRequest)
    {
      var result = await assignment.UpdateAssignment(assignmentId, assignmentRequest);
      return Ok(result);
    }
    
    
    [HttpDelete("assignmentId/{assignmentId}/archive")]
    public async Task<ActionResult<AssignmentResponse>> ArchiveAssignment([FromRoute] Guid assignmentId)
    {
      var result = await assignment.ArchiveAssignment(assignmentId);
      return Ok(result);
    }

    [HttpPut("archive/bulk")]
    public async Task<ActionResult<AssignmentsResponse>> ArchiveAssignments(List<Guid>? assignmentIds)
    {
      var result = await assignment.ArchiveAssignments(assignmentIds);
      return Ok(result);
    }
  }
}