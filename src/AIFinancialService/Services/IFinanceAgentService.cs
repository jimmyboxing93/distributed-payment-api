namespace AIFinancialService.Services
{
	public interface IFinanceAgentService
	{
		//Task<string> GetResponseAsync(string userMessage, CancellationToken ct = default);
		Task<string> GetAiResponseAsync(Guid sessionId, string userMessage);

		// To reset conversation
		Task ResetChatAsync(Guid sessionId);

		IAsyncEnumerable<string> StreamFinanceAssistResponse(Guid sessionId ,string prompt);

	}
}
