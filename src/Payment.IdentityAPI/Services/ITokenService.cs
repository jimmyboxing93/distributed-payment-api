using SharedData.Models;

namespace Payment.ClientView.Services
{
	public interface ITokenService
	{

		string CreateToken(User user);
	}
}
