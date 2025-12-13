using System;

namespace Server.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public int GroupId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }      // Понедельник = 1, Вторник = 2 и т.д.
        public bool IsNumerator { get; set; }         // true = числитель, false = знаменатель

        public string Time { get; set; } = null!;     // например "08:30–10:05"
        public string Subject { get; set; } = null!;
        public string Teacher { get; set; } = null!;
        public string Room { get; set; } = string.Empty;

        public Group Group { get; set; } = null!;
    }
}