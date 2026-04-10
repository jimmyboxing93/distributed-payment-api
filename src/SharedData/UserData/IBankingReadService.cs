using SharedData.Models;

namespace SharedData.UserData.Interfaces
{
	public interface IBankingReadService
	{
		UserInfo GetCreditCard(Guid id);
	}
}
