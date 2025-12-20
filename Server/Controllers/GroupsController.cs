using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(ApplicationDbContext db, ILogger<GroupsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET api/groups — список всех групп
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var groups = await _db.Groups.ToListAsync();
                _logger.LogInformation($"Получено {groups.Count} групп");
                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка групп");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // POST api/groups — добавить группу
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Group group)
        {
            try
            {
                _logger.LogInformation($"Добавление группы: {group.Name}");

                if (string.IsNullOrWhiteSpace(group.Name))
                {
                    return BadRequest(new { error = "Название группы не может быть пустым" });
                }

                if (await _db.Groups.AnyAsync(g => g.Name == group.Name))
                {
                    _logger.LogWarning($"Попытка добавить существующую группу: {group.Name}");
                    return BadRequest(new { error = "Группа с таким названием уже существует" });
                }

                _db.Groups.Add(group);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Группа добавлена: ID={group.Id}, Name={group.Name}");
                return Ok(group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении группы: {group.Name}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        // DELETE api/groups/{id} — удалить группу
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Удаление группы ID {id}");

                var group = await _db.Groups.FindAsync(id);
                if (group == null)
                {
                    _logger.LogWarning($"Группа ID {id} не найдена");
                    return NotFound(new { error = "Группа не найдена" });
                }

                // Проверяем, есть ли занятия у группы
                var hasLessons = await _db.Lessons.AnyAsync(l => l.GroupId == id);
                if (hasLessons)
                {
                    _logger.LogWarning($"Нельзя удалить группу {group.Name} - есть занятия");
                    return BadRequest(new { error = "Нельзя удалить группу, у которой есть занятия. Сначала удалите все занятия." });
                }

                _db.Groups.Remove(group);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Группа удалена: ID={id}, Name={group.Name}");
                return Ok(new
                {
                    success = true,
                    message = "Группа удалена успешно",
                    groupId = id,
                    groupName = group.Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении группы ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        // GET api/groups/{id}/lessons — получить занятия группы
        [HttpGet("{id}/lessons")]
        public async Task<IActionResult> GetGroupLessons(int id)
        {
            try
            {
                var group = await _db.Groups.FindAsync(id);
                if (group == null)
                {
                    return NotFound(new { error = $"Группа с ID {id} не найдена" });
                }

                var lessons = await _db.Lessons
                    .Where(l => l.GroupId == id)
                    .ToListAsync();

                _logger.LogInformation($"Получено {lessons.Count} занятий для группы ID {id}");
                return Ok(new
                {
                    groupId = id,
                    groupName = group.Name,
                    count = lessons.Count,
                    lessons = lessons
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении занятий группы ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }
    }
}