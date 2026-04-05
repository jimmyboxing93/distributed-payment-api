namespace SharedData.Models
{
	public class ChatMessageRecord
	{
		public Guid Id { get; set; }
		public Guid ChatSessionId { get; set; }
		public string Role { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		
	}
}
