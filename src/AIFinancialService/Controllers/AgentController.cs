using AIFinancialService.Services;
using Microsoft.AspNetCore.Mvc;
using AIFinancialService.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;


namespace AIFinancialService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AgentController : ControllerBase
	{
		private readonly IChatHistoryService _chatHistoryService;
		private readonly IFinanceAgentService _financeAgentService;
		
		public AgentController(IChatHistoryService chatHistoryService, IFinanceAgentService financeAgentService)
		{
			
			_chatHistoryService = chatHistoryService;
			_financeAgentService = financeAgentService;
		}

		[HttpGet("history/{sessionId}")]
		public async Task<IActionResult> GetChatHistory(Guid sessionId)
		{

			var history = await _chatHistoryService.GetProjectHistoryAsync(sessionId);

			return Ok(history);
		}

		[HttpPost("chat")]
		public IAsyncEnumerable<string> Chat([FromBody] ChatRequest request ) 
		{
			if (request == null || string.IsNullOrWhiteSpace(request.UserMessage)) 
			{
				throw new BadHttpRequestException("Message cannot be empty");
			}

			if (!Guid.TryParse(request.UserId, out Guid userGuid)) 
			{
				throw new BadHttpRequestException("Invalid User ID format. Please provide a valid GUID.");
			}
			// Get gemini response
			try
			{
				return _financeAgentService.StreamFinanceAssistResponse(request.SessionId ,request.UserMessage, userGuid);
			}
			catch (Exception) 
			{
				throw;
			}
		}



	}
}
