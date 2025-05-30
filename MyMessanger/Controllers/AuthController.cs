using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMessanger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static readonly List<User> _users = new List<User>();
        public static List<User> Users => _users;

        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthRequest request)
        {
            if (request == null)
                return BadRequest("Неверный запрос");

            if (_users.Any(u => u.Username == request.Username))
                return BadRequest("Пользователь уже существует");

            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _users.Add(user);
            return Ok(new { Token = user.Token });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            if (request == null)
                return BadRequest("Неверный запрос");

            var user = _users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Неверные учетные данные");

            user.Token = Guid.NewGuid().ToString();
            user.TokenExpiry = DateTime.UtcNow.AddHours(24);

            return Ok(new { Token = user.Token });
        }
    }

    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public DateTime TokenExpiry { get; set; } = DateTime.UtcNow.AddHours(24);
    }

    public class AuthRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}