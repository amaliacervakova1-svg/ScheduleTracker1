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
    public class LessonService : ILessonService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LessonService> _logger;
        private readonly IGroupService _groupService;

        public LessonService(
            ApplicationDbContext context,
            ILogger<LessonService> logger,
            IGroupService groupService)
        {
            _context = context;
            _logger = logger;
            _groupService = groupService;
        }

        public async Task<List<Lesson>> GetLessonsAsync(string groupName, DayOfWeek dayOfWeek, bool isNumerator)
        {
            try
            {
                _logger.LogInformation($"Получение занятий: группа={groupName}, день={dayOfWeek}, числитель={isNumerator}");

                var group = await _context.Groups
                    .FirstOrDefaultAsync(g => g.Name == groupName);

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
                    .Include(l => l.Group)
                    .ToListAsync();

                _logger.LogInformation($"Найдено {lessons.Count} занятий");
                return lessons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении занятий для группы {groupName}");
                throw;
            }
        }

        public async Task<List<Lesson>> GetAllLessonsAsync()
        {
            try
            {
                _logger.LogInformation("Получение всех занятий");

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
                _logger.LogError(ex, "Ошибка при получении всех занятий");
                throw;
            }
        }

        public async Task<Lesson?> GetLessonByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Получение занятия ID: {id}");

                return await _context.Lessons
                    .Include(l => l.Group)
                    .FirstOrDefaultAsync(l => l.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении занятия ID: {id}");
                throw;
            }
        }

        public async Task<List<Lesson>> GetLessonsByGroupIdAsync(int groupId)
        {
            try
            {
                _logger.LogInformation($"Получение занятий группы ID: {groupId}");

                return await _context.Lessons
                    .Where(l => l.GroupId == groupId)
                    .OrderBy(l => l.DayOfWeek)
                    .ThenBy(l => l.Time)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении занятий группы ID: {groupId}");
                throw;
            }
        }

        public async Task<Lesson> CreateLessonAsync(CreateLessonDto lessonDto)
        {
            try
            {
                _logger.LogInformation($"Создание занятия для группы ID: {lessonDto.GroupId}");

                // Проверяем существование группы
                var groupExists = await _groupService.GroupExistsAsync(lessonDto.GroupId);
                if (!groupExists)
                {
                    throw new ArgumentException($"Группа с ID {lessonDto.GroupId} не найдена");
                }

                // Проверяем дубликаты
                if (await IsDuplicateLessonAsync(lessonDto))
                {
                    throw new InvalidOperationException("Занятие с такими параметрами уже существует");
                }

                // Создаем занятие
                var lesson = new Lesson
                {
                    GroupId = lessonDto.GroupId,
                    DayOfWeek = (DayOfWeek)lessonDto.DayOfWeek,
                    IsNumerator = lessonDto.IsNumerator,
                    Time = lessonDto.Time,
                    Subject = lessonDto.Subject,
                    Teacher = lessonDto.Teacher,
                    Room = lessonDto.Room ?? string.Empty
                };

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();

                // Загружаем связанные данные для возврата
                await _context.Entry(lesson)
                    .Reference(l => l.Group)
                    .LoadAsync();

                _logger.LogInformation($"Создано занятие ID: {lesson.Id}");
                return lesson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при создании занятия для группы ID: {lessonDto.GroupId}");
                throw;
            }
        }

        public async Task<Lesson?> UpdateLessonAsync(int id, UpdateLessonDto lessonDto)
        {
            try
            {
                _logger.LogInformation($"Обновление занятия ID: {id}");

                var existingLesson = await _context.Lessons
                    .Include(l => l.Group)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (existingLesson == null)
                {
                    _logger.LogWarning($"Занятие ID {id} не найдено");
                    return null;
                }

                // Проверяем дубликаты (исключая текущее занятие)
                var createDto = new CreateLessonDto
                {
                    GroupId = lessonDto.GroupId,
                    DayOfWeek = lessonDto.DayOfWeek,
                    IsNumerator = lessonDto.IsNumerator,
                    Time = lessonDto.Time,
                    Subject = lessonDto.Subject,
                    Teacher = lessonDto.Teacher,
                    Room = lessonDto.Room
                };

                if (await IsDuplicateLessonAsync(createDto, id))
                {
                    throw new InvalidOperationException("Занятие с такими параметрами уже существует");
                }

                // Обновляем свойства
                existingLesson.GroupId = lessonDto.GroupId;
                existingLesson.DayOfWeek = (DayOfWeek)lessonDto.DayOfWeek;
                existingLesson.IsNumerator = lessonDto.IsNumerator;
                existingLesson.Time = lessonDto.Time;
                existingLesson.Subject = lessonDto.Subject;
                existingLesson.Teacher = lessonDto.Teacher;
                existingLesson.Room = lessonDto.Room ?? string.Empty;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Обновлено занятие ID: {id}");
                return existingLesson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении занятия ID: {id}");
                throw;
            }
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Удаление занятия ID: {id}");

                var lesson = await _context.Lessons.FindAsync(id);
                if (lesson == null)
                {
                    _logger.LogWarning($"Занятие ID {id} не найдено");
                    return false;
                }

                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Удалено занятие ID: {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении занятия ID: {id}");
                throw;
            }
        }

        public async Task<int> DeleteLessonsByGroupIdAsync(int groupId)
        {
            try
            {
                _logger.LogInformation($"Удаление всех занятий группы ID: {groupId}");

                var lessons = await _context.Lessons
                    .Where(l => l.GroupId == groupId)
                    .ToListAsync();

                if (!lessons.Any())
                {
                    _logger.LogInformation($"Для группы ID {groupId} нет занятий");
                    return 0;
                }

                _context.Lessons.RemoveRange(lessons);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Удалено {lessons.Count} занятий группы ID: {groupId}");
                return lessons.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении занятий группы ID: {groupId}");
                throw;
            }
        }

        public async Task<bool> LessonExistsAsync(int id)
        {
            return await _context.Lessons.AnyAsync(l => l.Id == id);
        }

        public async Task<bool> IsDuplicateLessonAsync(CreateLessonDto lessonDto, int? excludeId = null)
        {
            var query = _context.Lessons
                .Where(l => l.GroupId == lessonDto.GroupId &&
                            l.DayOfWeek == (DayOfWeek)lessonDto.DayOfWeek &&
                            l.Time == lessonDto.Time &&
                            l.IsNumerator == lessonDto.IsNumerator);

            if (excludeId.HasValue)
            {
                query = query.Where(l => l.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<int> GetLessonsCountAsync()
        {
            return await _context.Lessons.CountAsync();
        }

        public async Task<int> GetLessonsCountByGroupAsync(int groupId)
        {
            return await _context.Lessons
                .Where(l => l.GroupId == groupId)
                .CountAsync();
        }
    }
}