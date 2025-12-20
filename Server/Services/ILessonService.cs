using Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface ILessonService
    {
        // Получение занятий
        Task<List<Lesson>> GetLessonsAsync(string groupName, DayOfWeek dayOfWeek, bool isNumerator);
        Task<List<Lesson>> GetAllLessonsAsync();
        Task<Lesson?> GetLessonByIdAsync(int id);
        Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId);

        // Создание, обновление, удаление
        Task<Lesson> CreateLessonAsync(CreateLessonDto lessonDto);
        Task<Lesson?> UpdateLessonAsync(int id, UpdateLessonDto lessonDto);
        Task<bool> DeleteLessonAsync(int id);
        Task<int> DeleteLessonsByGroupIdAsync(int groupId);

        // Валидация и проверки
        Task<bool> LessonExistsAsync(int id);
        Task<bool> IsDuplicateLessonAsync(CreateLessonDto lessonDto, int? excludeId = null);

        // Статистика
        Task<int> GetLessonsCountAsync();
        Task<int> GetLessonsCountByGroupAsync(int groupId);
    }

    // DTO для обновления занятия
    public class UpdateLessonDto
    {
        public int GroupId { get; set; }
        public int DayOfWeek { get; set; }
        public bool IsNumerator { get; set; }
        public string Time { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Teacher { get; set; } = null!;
        public string Room { get; set; } = string.Empty;
    }
}