using Microsoft.SemanticKernel;
using System.ComponentModel;
using SharedData.UserData.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AIFinancialService.Plugins
{
	public class BankingInfo
	{
		private readonly IBankingReadService _readOnlyUserInfo;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public BankingInfo(IBankingReadService readOnlyUserInfo, IHttpContextAccessor httpContextAccessor) 
		{
			_readOnlyUserInfo = readOnlyUserInfo;
			_httpContextAccessor = httpContextAccessor;
		}

		[KernelFunction]
		[Description("Retrieves the logged-in user's general banking info, financial account details, balances, and credit card statement metrics.")]
		public async Task<string> GetCreditCardDetailsById(Kernel kernel)
			{
			var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

			if (claim == null || !Guid.TryParse(claim.Value, out var guidId))
			{
				return "Unauthorized: System user context is missing or invalid in JWT.";
			}

			var user = _readOnlyUserInfo.GetCreditCard(guidId);

			if (user == null)
			{
				return $"System Error: Profile records not found for user context identity '{guidId}'.";
			}

			return  $"Account Holder: {user.FirstName} {user.LastName}, | " +
					$"Card ending in: {user.LastFourDigits}, | " +
					$"Current amount: {user.amount:C}, " +
					$"Expiry: {user.expirationDate:MM/yy}" ;
			}
		
	}
}
