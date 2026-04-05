using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using SharedData.Models;
using AIFinancialService.Services;
using System.Text;

namespace AIFinancialService.Services
{
	public class FinanceAgentSerivce : IFinanceAgentService
	{
		private readonly Kernel _kernel;
		private readonly IChatCompletionService _chatService;
		private readonly IChatHistoryService _historyService;


		public FinanceAgentSerivce(Kernel kernel, IChatCompletionService chatCompletion, IChatHistoryService historyService) 
		{
			_kernel = kernel;
			_chatService = kernel.GetRequiredService<IChatCompletionService>();
			_historyService = historyService;

		}

		public async Task<string> GetAiResponseAsync(Guid sessionId, string userMessage) 
		{
			await _historyService.EnsureSessionExistsAsync(sessionId);

			await _historyService.SaveMessageAsync(new ChatMessageRecord
			{
				ChatSessionId = sessionId,
				Role = "User",
				Content = userMessage,
				CreatedAt = DateTime.UtcNow

			});

			var dbMessage = await _historyService.GetProjectHistoryAsync(sessionId);

			var history = new ChatHistory("You are a Senior Financial Assistant. Use provided tools to help customers.");


			if (dbMessage.Any())
			{
				foreach (var message in dbMessage)
				{
					var role = string.Equals(message.Role, "User", StringComparison.OrdinalIgnoreCase) ? AuthorRole.User : AuthorRole.Assistant;

					history.AddMessage(role, message.Content);
				}
			}


			var settings = new GeminiPromptExecutionSettings
			{
				ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions
			};


			var response = await _chatService.GetChatMessageContentAsync(history, settings, _kernel);

			
			
			string assistantContent = response.ToString();

			

			await _historyService.SaveMessageAsync(new ChatMessageRecord
			{
				ChatSessionId = sessionId,
				Role = "assistant",
				Content = assistantContent,
				CreatedAt = DateTime.UtcNow

			});

			string usageStats = "Usage data not available";

			if (response.Metadata != null && response.Metadata.TryGetValue("Usage", out var usage))
			{
				usageStats = usage?.ToString() ?? "Empty usage";
			}

			Console.WriteLine($"AI Raw Response: {assistantContent}");
			Console.WriteLine($"Metadata Stats: {usageStats}");


			return assistantContent;
		}

		public async Task ResetChatAsync(Guid sessionId) 
		{
			await _historyService.ClearSessionHistoryAsync(sessionId);
			Console.WriteLine($"Session {sessionId} has been reset in the database.");
		}

		public async IAsyncEnumerable<string> StreamFinanceAssistResponse(Guid sessionId, string prompt)
		{
			await _historyService.EnsureSessionExistsAsync(sessionId);
			var function = _kernel.CreateFunctionFromPrompt(prompt);
			var fullResponse = new StringBuilder();

			var settings = new GeminiPromptExecutionSettings
			{
				ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions
			};


			await foreach (var chunk in _kernel.InvokeStreamingAsync<StreamingChatMessageContent>(function, new KernelArguments(settings)))
			{
				if (chunk.Content is not null)
				{
					fullResponse.Append(chunk.Content);

					// Pushes one chunk at a time and only return one it is complete. 
					yield return chunk.Content;
				}

				if (chunk.Metadata != null && chunk.Metadata.TryGetValue("Usage", out var usage))
				{
					Console.WriteLine($"Tokens usage: {usage}");
				}
			}

			await _historyService.SaveMessageAsync( new ChatMessageRecord
			{
				ChatSessionId = sessionId,
				Role = "Assistant", 
				Content = fullResponse.ToString(),
				CreatedAt = DateTime.UtcNow,
			});
		}

	}
}
