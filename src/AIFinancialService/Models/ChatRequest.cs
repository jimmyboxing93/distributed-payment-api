using System.Text.Json.Serialization;

namespace AIFinancialService.Models
{
	public class ChatRequest
	{
		[JsonPropertyName("SessionId")]
		public Guid SessionId { get; set; }
		[JsonPropertyName("UserMessage")]
		public string UserMessage { get; set; } = string.Empty;

		[JsonPropertyName("UserId")]
		public string? UserId { get; set; }
	}
}
