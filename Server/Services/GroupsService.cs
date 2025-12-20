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
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GroupService> _logger;

        public GroupService(ApplicationDbContext context, ILogger<GroupService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            try
            {
                _logger.LogInformation("Получение всех групп");
                return await _context.Groups.OrderBy(g => g.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех групп");
                throw;
            }
        }

        public async Task<Group?> GetGroupByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Получение группы по ID: {id}");
                return await _context.Groups.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении группы ID: {id}");
                throw;
            }
        }

        public async Task<Group> AddGroupAsync(Group group)
        {
            try
            {
                _logger.LogInformation($"Добавление новой группы: {group.Name}");

                // Проверяем, существует ли уже группа с таким именем
                var existingGroup = await _context.Groups
                    .FirstOrDefaultAsync(g => g.Name == group.Name);

                if (existingGroup != null)
                {
                    _logger.LogWarning($"Группа с именем {group.Name} уже существует");
                    throw new InvalidOperationException($"Группа с именем '{group.Name}' уже существует");
                }

                _context.Groups.Add(group);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Группа добавлена: ID={group.Id}, Name={group.Name}");
                return group;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении группы: {group.Name}");
                throw;
            }
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Удаление группы ID: {id}");

                var group = await _context.Groups.FindAsync(id);
                if (group == null)
                {
                    _logger.LogWarning($"Группа ID {id} не найдена");
                    return false;
                }

                // Проверяем, есть ли занятия у группы
                bool hasLessons = await _context.Lessons.AnyAsync(l => l.GroupId == id);
                if (hasLessons)
                {
                    _logger.LogWarning($"Нельзя удалить группу {group.Name} - есть занятия");
                    throw new InvalidOperationException($"Нельзя удалить группу '{group.Name}', так как у неё есть занятия. Сначала удалите все занятия.");
                }

                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Группа удалена: ID={id}, Name={group.Name}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении группы ID: {id}");
                throw;
            }
        }

        public async Task<bool> GroupExistsAsync(int id)
        {
            return await _context.Groups.AnyAsync(g => g.Id == id);
        }

        public async Task<bool> GroupHasLessonsAsync(int id)
        {
            return await _context.Lessons.AnyAsync(l => l.GroupId == id);
        }

        public async Task<List<Lesson>> GetGroupLessonsAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Получение занятий группы ID: {id}");
                return await _context.Lessons
                    .Where(l => l.GroupId == id)
                    .OrderBy(l => l.DayOfWeek)
                    .ThenBy(l => l.Time)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении занятий группы ID: {id}");
                throw;
            }
        }

        public async Task<Group?> UpdateGroupAsync(int id, Group updatedGroup)
        {
            try
            {
                _logger.LogInformation($"Обновление группы ID: {id}");

                var existingGroup = await _context.Groups.FindAsync(id);
                if (existingGroup == null)
                {
                    _logger.LogWarning($"Группа ID {id} не найдена для обновления");
                    return null;
                }

                // Проверяем, не пытаемся ли изменить имя на уже существующее
                if (existingGroup.Name != updatedGroup.Name)
                {
                    var duplicate = await _context.Groups
                        .FirstOrDefaultAsync(g => g.Name == updatedGroup.Name && g.Id != id);

                    if (duplicate != null)
                    {
                        _logger.LogWarning($"Группа с именем {updatedGroup.Name} уже существует");
                        throw new InvalidOperationException($"Группа с именем '{updatedGroup.Name}' уже существует");
                    }
                }

                // Обновляем свойства
                existingGroup.Name = updatedGroup.Name;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Группа обновлена: ID={id}, Name={existingGroup.Name}");

                return existingGroup;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении группы ID: {id}");
                throw;
            }
        }
    }
}