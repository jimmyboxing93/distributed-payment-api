using Microsoft.AspNetCore.Identity;

namespace SharedData.Models;

public class User : IdentityUser<Guid>
{
	public string UserPassword { get; set; } = string.Empty;
}