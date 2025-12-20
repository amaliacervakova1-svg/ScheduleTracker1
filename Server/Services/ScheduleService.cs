using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Data;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(ApplicationDbContext context, ILogger<ScheduleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Lesson>> GetScheduleAsync(string groupName, DayOfWeek dayOfWeek, bool isNumerator)
        {
            try
            {
                var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
                if (group == null)
                {
                    _logger.LogWarning($"Группа '{groupName}' не найдена");
                    return new List<Lesson>();
                }

                var lessons = await _context.Lessons
                    .Where(l => l.GroupId == group.Id &&
                                l.DayOfWeek == dayOfWeek &&
                                l.IsNumerator == isNumerator)
                    .OrderBy(l => l.Time)
                    .ToListAsync();

                _logger.LogInformation($"Найдено {lessons.Count} занятий для группы {groupName}, день {dayOfWeek}, неделя {(isNumerator ? "числитель" : "знаменатель")}");
                return lessons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении расписания для группы {groupName}");
                throw;
            }
        }

        public async Task<List<Lesson>> GetAllSchedulesAsync()
        {
            try
            {
                var lessons = await _context.Lessons
                    .Include(l => l.Group)
                    .OrderBy(l => l.Group.Name)
                    .ThenBy(l => l.DayOfWeek)
                    .ThenBy(l => l.Time)
                    .ToListAsync();

                _logger.LogInformation($"Загружено {lessons.Count} занятий");
                return lessons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех расписаний");
                throw;
            }
        }

        public async Task<Lesson> AddLessonAsync(Lesson lesson)
        {
            try
            {
                // Проверяем существование группы
                var groupExists = await _context.Groups.AnyAsync(g => g.Id == lesson.GroupId);
                if (!groupExists)
                {
                    throw new ArgumentException($"Группа с ID {lesson.GroupId} не найдена");
                }

                // Проверяем уникальность (группа + день + время + тип недели)
                var duplicateExists = await _context.Lessons
                    .AnyAsync(l => l.GroupId == lesson.GroupId &&
                                   l.DayOfWeek == lesson.DayOfWeek &&
                                   l.Time == lesson.Time &&
                                   l.IsNumerator == lesson.IsNumerator);

                if (duplicateExists)
                {
                    throw new InvalidOperationException("Занятие с такими параметрами уже существует");
                }

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Добавлено занятие ID {lesson.Id} для группы ID {lesson.GroupId}");
                return lesson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении занятия для группы ID {lesson.GroupId}");
                throw;
            }
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            try
            {
                var lesson = await _context.Lessons.FindAsync(id);
                if (lesson == null)
                {
                    _logger.LogWarning($"Занятие ID {id} не найдено для удаления");
                    return false;
                }

                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Удалено занятие ID {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении занятия ID {id}");
                throw;
            }
        }

        public async Task<int> DeleteAllLessonsForGroupAsync(int groupId)
        {
            try
            {
                // Проверяем существование группы
                var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
                if (!groupExists)
                {
                    throw new ArgumentException($"Группа с ID {groupId} не найдена");
                }

                var groupLessons = await _context.Lessons
                    .Where(l => l.GroupId == groupId)
                    .ToListAsync();

                if (!groupLessons.Any())
                {
                    _logger.LogInformation($"Для группы ID {groupId} нет занятий для удаления");
                    return 0;
                }

                _context.Lessons.RemoveRange(groupLessons);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Удалено {groupLessons.Count} занятий для группы ID {groupId}");
                return groupLessons.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении занятий для группы ID {groupId}");
                throw;
            }
        }

        public async Task<Lesson?> GetLessonByIdAsync(int id)
        {
            try
            {
                return await _context.Lessons
                    .Include(l => l.Group)
                    .FirstOrDefaultAsync(l => l.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении занятия ID {id}");
                throw;
            }
        }

        public async Task<Lesson?> UpdateLessonAsync(int id, Lesson updatedLesson)
        {
            try
            {
                var existingLesson = await _context.Lessons.FindAsync(id);
                if (existingLesson == null)
                {
                    return null;
                }

                // Обновляем поля
                existingLesson.DayOfWeek = updatedLesson.DayOfWeek;
                existingLesson.IsNumerator = updatedLesson.IsNumerator;
                existingLesson.Time = updatedLesson.Time;
                existingLesson.Subject = updatedLesson.Subject;
                existingLesson.Teacher = updatedLesson.Teacher;
                existingLesson.Room = updatedLesson.Room;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Обновлено занятие ID {id}");

                return existingLesson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении занятия ID {id}");
                throw;
            }
        }
    }
}