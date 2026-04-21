using AIFinancialService.Services;
using Microsoft.AspNetCore.Mvc;
using AIFinancialService.Models;
using UglyToad.PdfPig;
using System.Text;


namespace AIFinancialService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AgentController : ControllerBase
	{
		private readonly IChatHistoryService _chatHistoryService;
		private readonly IFinanceAgentService _financeAgentService;
		private readonly KnowledgeService _knowledgeService;
		
		public AgentController(IChatHistoryService chatHistoryService, IFinanceAgentService financeAgentService, KnowledgeService knowledgeService)
		{
			
			_chatHistoryService = chatHistoryService;
			_financeAgentService = financeAgentService;
			_knowledgeService = knowledgeService;
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

		[HttpPost("upload-policy")]
		public async Task<IActionResult> PolicyResult(IFormFile file) 
		{
			if (file == null || file.Length == 0) return BadRequest("No file uploaded");

			var textBuilder = new StringBuilder();

			using (var stream = file.OpenReadStream())
			using (var pdf = PdfDocument.Open(stream))
			{
				foreach (var page in pdf.GetPages()) 
				{
					// PdfPig handles the text extraction page by page
					textBuilder.AppendLine(page.Text);
				}
			}

			var fullText = textBuilder.ToString();

			if (string.IsNullOrWhiteSpace(fullText))
				return BadRequest("Could not extract text from pdf.");

			await _knowledgeService.IngestPolicyAsync(fullText);

			return Ok(new
			{
				Message = "Policy Ingested successfully!",
				CharacterCount = fullText.Length
			});
		}


	}
}
