namespace MyMessanger.Models
{
    public class SendMessageRequest
    {
        public required string Message { get; set; }
        public required string Token { get; set; }
    }
}
