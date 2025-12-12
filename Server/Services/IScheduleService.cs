using Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IScheduleService
    {
        Task<List<Lesson>> GetScheduleAsync(string groupName, DayOfWeek dayOfWeek, bool isNumerator);
        Task<List<Lesson>> GetAllSchedulesAsync();
        Task<Lesson> AddLessonAsync(Lesson lesson); // Изменено на Task<Lesson>
        Task<bool> DeleteLessonAsync(int id); // Изменено на Task<bool>
        Task<int> DeleteAllLessonsForGroupAsync(int groupId);
        Task<Lesson?> GetLessonByIdAsync(int id); // Новый метод
        Task<Lesson?> UpdateLessonAsync(int id, Lesson updatedLesson); // Новый метод
    }
}