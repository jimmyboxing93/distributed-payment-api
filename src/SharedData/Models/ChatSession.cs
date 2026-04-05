namespace SharedData.Models
{
	public class ChatSession
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		// Links to exisiting user table
		public Guid UserId { get; set; }
		public string Title { get; set; } = "New Analysis";
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public List<ChatMessageRecord> Messages { get; set; } = new();



	}
}
