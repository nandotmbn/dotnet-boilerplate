using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models
{
	public class User : Mandatory
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required, MaxLength(20)]
		public string? FirstName { get; set; } = string.Empty;
		[Required, MaxLength(20)]
		public string? LastName { get; set; } = string.Empty;
		[Required, EmailAddress, MaxLength(64), MinLength(5)]
		public string? Email { get; set; } = string.Empty;
		[Required, EmailAddress, MaxLength(64)]
		public string? Username { get; set; } = string.Empty;
		[JsonIgnore, MaxLength(64), MinLength(8)]
		public string? Password { get; set; } = string.Empty;
	}
}

