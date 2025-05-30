namespace MyMessanger.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Text { get; set; }
        public required string Username { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
