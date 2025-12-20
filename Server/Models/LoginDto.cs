using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = null!;
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; } // для будущего использования JWT
    }
}