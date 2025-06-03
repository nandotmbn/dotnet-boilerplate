using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class UserShard : Mandatory
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Username { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		[Required, Range(0, int.MaxValue)]
		public int Shard { get; set; } = 0;
	}
}

