using Microsoft.AspNetCore.Identity;

namespace Payment.ClientView.Services
{
	public interface ITokenService
	{

		string CreateToken(IdentityUser user);
	}
}
