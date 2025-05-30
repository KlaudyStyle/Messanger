using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMessanger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private static readonly List<Message> _messages = new List<Message>();

        [HttpPost]
        public IActionResult SendMessage([FromBody] SendMessageRequest request)
        {
            if (request == null)
                return BadRequest("Неверный запрос");

            var user = AuthController.Users.FirstOrDefault(u =>
                u.Token == request.Token && u.TokenExpiry > DateTime.UtcNow);

            if (user == null)
                return Unauthorized("Недействительный токен");

            var message = new Message
            {
                Text = request.Message,
                Username = user.Username
            };

            _messages.Add(message);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetMessages()
        {
            return Ok(_messages);
        }
    }

    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class SendMessageRequest
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}