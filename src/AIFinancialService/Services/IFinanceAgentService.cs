namespace AIFinancialService.Services
{
	public interface IFinanceAgentService
	{
		
		Task<string> GetAiResponseAsync(Guid sessionId, string userMessage, Guid userId);

		// To reset conversation
		Task ResetChatAsync(Guid sessionId);

		IAsyncEnumerable<string> StreamFinanceAssistResponse(Guid sessionId ,string prompt, Guid userId);

	}
}
