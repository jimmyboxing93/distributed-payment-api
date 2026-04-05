using SharedData.Data;
using SharedData.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AIFinancialService.Services
{
	public class ChatHistoryService : IChatHistoryService
	{
		private readonly SeniorDbContext _context;

		public ChatHistoryService(SeniorDbContext context) 
		{
			_context = context;
		}

		public async Task<List<ChatMessageRecord>> GetProjectHistoryAsync(Guid sessionId) 
		{

			
			return await _context.ChatMessages
			.Where(m => m.ChatSessionId == sessionId)
			.OrderBy(m => m.CreatedAt)
			.ToListAsync();
		}

		public async Task EnsureSessionExistsAsync(Guid sessionId)
		{
			var session = await _context.ChatSessions.FindAsync(sessionId);

			if (session == null)
			{
				_context.ChatSessions.Add(new ChatSession
				{
					Id = sessionId,
					CreatedAt = DateTime.UtcNow,
				});
				await _context.SaveChangesAsync();
			}
		}

		public async Task SaveMessageAsync(ChatMessageRecord chatMessageRecord) 
		{

			if (chatMessageRecord.Id == Guid.Empty) 
			{
				chatMessageRecord.Id = Guid.NewGuid();
			}
			_context.ChatMessages.Add(chatMessageRecord);

			await _context.SaveChangesAsync();

			_context.Entry(chatMessageRecord).State = EntityState.Detached;
		}

		public async Task SaveMessageAsync(Guid sessionId, string role, string content) 
		{
			var record = new ChatMessageRecord
			{
				Id = Guid.NewGuid(),
				ChatSessionId = sessionId,
				Role = role,
				Content = content,
				CreatedAt = DateTime.UtcNow
			};

			await SaveMessageAsync(record);
		}

		public async Task ClearSessionHistoryAsync(Guid sessionId) 
		{
			await _context.ChatMessages
			.Where(m => m.ChatSessionId == sessionId)
			.ExecuteDeleteAsync();

		}


	}
}
