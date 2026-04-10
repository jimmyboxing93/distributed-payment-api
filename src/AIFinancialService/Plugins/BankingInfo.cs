using Microsoft.SemanticKernel;
using System.ComponentModel;
using SharedData.Interfaces;
using SharedData.UserData.Interfaces;

namespace AIFinancialService.Plugins
{
	public class BankingInfo
	{
		private readonly IBankingReadService _readOnlyUserInfo;

		public BankingInfo(IBankingReadService readOnlyUserInfo) 
		{
			_readOnlyUserInfo = readOnlyUserInfo;
		}

		[KernelFunction]
		[Description("Retrieves the logged-in user's credit card details.")]
		public async Task<string> GetCreditCardDetailsById(
			[Description("The system user context")] string userId)
			{

			if (!Guid.TryParse(userId, out var guidId))
			{
				return "Invalid user ID format.";
			}

			var user = _readOnlyUserInfo.GetCreditCard(guidId);

			if (user == null) return "User not found.";

			return  $"Account Holder: {user.FirstName} {user.LastName}, | " +
					$"Card ending in: {user.LastFourDigits}, | " +
					$"Current amount: {user.amount:C}, " +
					$"Expiry: {user.expirationDate:MM/yy}" ;
			}
		
	}
}
