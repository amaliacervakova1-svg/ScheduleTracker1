using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IGroupService groupService, ILogger<GroupsController> logger)
        {
            _groupService = groupService;
            _logger = logger;
        }

        // GET api/groups
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var groups = await _groupService.GetAllGroupsAsync();
                _logger.LogInformation($"Получено {groups.Count} групп");
                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка групп");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // GET api/groups/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var group = await _groupService.GetGroupByIdAsync(id);

                if (group == null)
                {
                    _logger.LogWarning($"Группа ID {id} не найдена");
                    return NotFound(new { error = "Группа не найдена" });
                }

                return Ok(group);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении группы ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // POST api/groups
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

                var addedGroup = await _groupService.AddGroupAsync(group);
                return Ok(addedGroup);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Конфликт при добавлении группы: {group.Name}");
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении группы: {group.Name}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        // DELETE api/groups/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Удаление группы ID {id}");

                // Проверяем существование группы
                var groupExists = await _groupService.GroupExistsAsync(id);
                if (!groupExists)
                {
                    _logger.LogWarning($"Группа ID {id} не найдена");
                    return NotFound(new { error = "Группа не найдена" });
                }

                // Пытаемся удалить группу
                var deleted = await _groupService.DeleteGroupAsync(id);

                if (!deleted)
                {
                    return BadRequest(new { error = "Не удалось удалить группу" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Группа удалена успешно",
                    groupId = id
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Ошибка операции при удалении группы ID {id}");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении группы ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        // GET api/groups/{id}/lessons
        [HttpGet("{id}/lessons")]
        public async Task<IActionResult> GetGroupLessons(int id)
        {
            try
            {
                // Проверяем существование группы
                var group = await _groupService.GetGroupByIdAsync(id);
                if (group == null)
                {
                    return NotFound(new { error = $"Группа с ID {id} не найдена" });
                }

                var lessons = await _groupService.GetGroupLessonsAsync(id);

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

        // PUT api/groups/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Group updatedGroup)
        {
            try
            {
                _logger.LogInformation($"Обновление группы ID {id}");

                if (string.IsNullOrWhiteSpace(updatedGroup.Name))
                {
                    return BadRequest(new { error = "Название группы не может быть пустым" });
                }

                var result = await _groupService.UpdateGroupAsync(id, updatedGroup);

                if (result == null)
                {
                    return NotFound(new { error = $"Группа с ID {id} не найдена" });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Конфликт при обновлении группы ID {id}");
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении группы ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }
    }
}