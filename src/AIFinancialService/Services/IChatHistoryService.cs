using Microsoft.Extensions.AI;
using SharedData.Models;

namespace AIFinancialService.Services
{
	public interface IChatHistoryService
	{
		Task<List<ChatMessageRecord>> GetProjectHistoryAsync(Guid sessionId);
		Task SaveMessageAsync (ChatMessageRecord record);
		Task ClearSessionHistoryAsync(Guid sessionId);
		Task EnsureSessionExistsAsync(Guid sessionId);
		Task SaveMessageAsync(Guid sessionId, string role, string content);
	}
}
