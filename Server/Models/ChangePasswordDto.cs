using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Текущий пароль обязателен")]
        public string OldPassword { get; set; } = null!;

        [Required(ErrorMessage = "Новый пароль обязателен")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
        public string NewPassword { get; set; } = null!;

        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = null!;
    }
}