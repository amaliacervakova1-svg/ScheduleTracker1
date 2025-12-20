using Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IScheduleService
    {
        Task<bool> DeleteLessonAsync(int id); // Изменено на Task<bool>
        Task<int> DeleteAllLessonsForGroupAsync(int groupId);
        Task<int> DeleteAllLessonsForGroupAsync(int groupId);
        Task<Lesson?> GetLessonByIdAsync(int id); // Новый метод
        Task<Lesson?> UpdateLessonAsync(int id, Lesson updatedLesson); // Новый метод
    }
}