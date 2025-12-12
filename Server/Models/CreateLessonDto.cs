using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class CreateLessonDto
    {
        [Required(ErrorMessage = "GroupId обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "GroupId должен быть положительным числом")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "DayOfWeek обязателен")]
        [Range(0, 6, ErrorMessage = "DayOfWeek должен быть от 0 до 6")]
        public int DayOfWeek { get; set; }

        [Required(ErrorMessage = "IsNumerator обязателен")]
        public bool IsNumerator { get; set; }

        [Required(ErrorMessage = "Время обязательно")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Время должно быть от 5 до 20 символов")]
        public string Time { get; set; } = null!;

        [Required(ErrorMessage = "Предмет обязателен")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название предмета должно быть от 2 до 100 символов")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Преподаватель обязателен")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Имя преподавателя должно быть от 3 до 100 символов")]
        public string Teacher { get; set; } = null!;

        [StringLength(20, ErrorMessage = "Аудитория не более 20 символов")]
        public string Room { get; set; } = string.Empty;
    }
}
//