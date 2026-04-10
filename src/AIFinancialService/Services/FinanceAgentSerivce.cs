using System.Text;
using AIFinancialService.Plugins;
using AIFinancialService.Services;
using Google.GenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using SharedData.Models;

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

		public async Task<string> GetAiResponseAsync(Guid sessionId, string userMessage, Guid userId) 
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

			var history = await BuildHistoryAsync(sessionId, userMessage, userId);


			var settings = new GeminiPromptExecutionSettings
			{
				ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions
			};

			var arguments = GetExecutionArguments(userId);

			_kernel.Data["userId"] = userId.ToString();


			var response = await _chatService.GetChatMessageContentAsync(history, settings, _kernel);



			string assistantContent = response.ToString();

			

			await _historyService.SaveMessageAsync(new ChatMessageRecord
			{
				ChatSessionId = sessionId,
				Role = "assistant",
				Content = assistantContent,
				CreatedAt = DateTime.UtcNow

			});

			//string usageStats = "Usage data not available";

			//if (response.Metadata != null && response.Metadata.TryGetValue("Usage", out var usage))
			//{
			//	usageStats = usage?.ToString() ?? "Empty usage";
			//}

			//Console.WriteLine($"AI Raw Response: {assistantContent}");
			//Console.WriteLine($"Metadata Stats: {usageStats}");


			return assistantContent;
		}

		public async Task ResetChatAsync(Guid sessionId) 
		{
			await _historyService.ClearSessionHistoryAsync(sessionId);
			Console.WriteLine($"Session {sessionId} has been reset in the database.");
		}

		public async IAsyncEnumerable<string> StreamFinanceAssistResponse(Guid sessionId, string prompt, Guid userId)
		{
			await _historyService.EnsureSessionExistsAsync(sessionId);

			

			
			var fullResponse = new StringBuilder();

			var history = await BuildHistoryAsync(sessionId, prompt, userId);

			var settings = new GeminiPromptExecutionSettings
			{
				ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions
			};

			// This makes it available to any Plugin  that gets called
			var args = GetExecutionArguments(userId);


			await foreach (var chunk in _chatService.GetStreamingChatMessageContentsAsync(history, settings, _kernel))
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

		private KernelArguments GetExecutionArguments(Guid userId)
		{
			var settings = new GeminiPromptExecutionSettings
			{
				ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions
			};

			return new KernelArguments(settings)
			{
				["userId"] = userId.ToString()
			};
		}

		private async Task<ChatHistory> BuildHistoryAsync(Guid sessionId, string userMessage, Guid userId)
		{
			var dbMessages = await _historyService.GetProjectHistoryAsync(sessionId);

			var history = new ChatHistory($"You are a helpful Financial Assistant. " +
						 $"The current logged-in User ID is: {userId}. " +
						 "Use this ID for any account-related tool calls. " +
						 "Address the user by name and NEVER repeat the GUID in your response.");

			foreach (var message in dbMessages)
			{
				var role = string.Equals(message.Role, "User", StringComparison.OrdinalIgnoreCase)
						   ? AuthorRole.User : AuthorRole.Assistant;
				history.AddMessage(role, message.Content);
			}

			history.AddUserMessage(userMessage);
			return history;
		}

	}
}
