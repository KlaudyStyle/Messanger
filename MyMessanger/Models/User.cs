namespace MyMessanger.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public DateTime TokenExpiry { get; set; } = DateTime.UtcNow.AddHours(24);
    }
}
