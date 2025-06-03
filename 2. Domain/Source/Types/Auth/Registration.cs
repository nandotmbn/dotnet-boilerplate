using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Models;

namespace Domain.Types
{
  public class RegistrationRequest
  {
    [Required, MaxLength(20)]
    public string? FirstName { get; set; } = string.Empty;
    [Required, MaxLength(20)]
    public string? LastName { get; set; } = string.Empty;
    [Required, MaxLength(64), MinLength(5)]
    public string? Username { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(64), MinLength(5)]
    public string? Email { get; set; } = string.Empty;
    [Required, MaxLength(64), MinLength(8)]
    public string? Password { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(64), Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; } = string.Empty;
  }

  public record RegistrationResponse(HttpStatusCode StatusCode, string Message, User? Data);
}