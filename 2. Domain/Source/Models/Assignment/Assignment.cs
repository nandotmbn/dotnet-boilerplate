using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models
{
	public class Assignment : Mandatory
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required]
		public Guid UserId { get; set; }
		[JsonIgnore]
		public User? User { get; set; } = null;
    
		public string? Title { get; set; } = string.Empty;
		public string? Description { get; set; } = string.Empty;
	}
}

