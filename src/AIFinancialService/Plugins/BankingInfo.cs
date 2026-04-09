using Microsoft.SemanticKernel;
using System.ComponentModel;
using SharedData.Interfaces;

namespace AIFinancialService.Plugins
{
	public class BankingInfo
	{
		private readonly IUserInfo _userInfo;

		public BankingInfo(IUserInfo userInfo) 
		{
			_userInfo = userInfo;
		}

		[KernelFunction]
		[Description("Retrieves full credit card details for a user based on their unique user ID.")]
		public async Task<string> GetCreditCardDetailsById(
			[Description("The unique GUID identifier for the user")] string userId) 
			{

				if (!Guid.TryParse(userId, out var guidId))
				{
					return $"I could not find a credit card record for that ID: {userId}";
				}

				var user = _userInfo.GetCreditCard(guidId);

			if (user == null) return "User not found.";

			return  $"User: {user.FirstName} {user.LastName}, " +
					$"Card ending in: {user.LastFourDigits}, " +
					$"Current amount: {user.amount:C}, " +
					$"Expiry: {user.expirationDate:MM/yy}" ;
			}
		
	}
}
