using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace MyMessanger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private static List<Message> messages = new List<Message>();

        [HttpGet]
        public ActionResult<IEnumerable<Message>> Get()
        {
            return Ok(messages);
        }

        [HttpPost]
        public ActionResult Post([FromBody] MessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text) ||
                string.IsNullOrWhiteSpace(request.UserName))
            {
                return BadRequest("Текст и имя пользователя обязательны");
            }

            var message = new Message
            {
                Id = Guid.NewGuid(),
                Text = request.Text,
                UserName = request.UserName,
                CreatedAt = DateTime.UtcNow
            };

            messages.Add(message);
            return Ok();
        }
    }

    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class MessageRequest
    {
        public string Text { get; set; }
        public string UserName { get; set; }
    }
}