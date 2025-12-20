using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILessonService lessonService, ILogger<ScheduleController> logger)
        {
            _lessonService = lessonService;
            _logger = logger;
        }

        // GET api/schedule
        [HttpGet]
        public async Task<IActionResult> GetSchedule(string group, int day, bool numerator)
        {
            try
            {
                _logger.LogInformation($"Запрос расписания: группа={group}, день={day}, числитель={numerator}");

                if (!Enum.IsDefined(typeof(DayOfWeek), day))
                {
                    return BadRequest(new { error = "Неверный день недели (0-6)" });
                }

                var lessons = await _lessonService.GetLessonsAsync(group, (DayOfWeek)day, numerator);
                return Ok(lessons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении расписания для группы {group}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // GET api/schedule/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSchedules()
        {
            try
            {
                var all = await _lessonService.GetAllLessonsAsync();
                return Ok(all);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех расписаний");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // POST api/schedule
        [HttpPost]
        public async Task<IActionResult> AddLesson([FromBody] Lesson lesson)
        {
            try
            {
                _logger.LogInformation($"Добавление занятия для группы ID {lessonDto.GroupId}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var addedLesson = await _lessonService.CreateLessonAsync(lessonDto);
                return Ok(addedLesson);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Ошибка аргумента: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Конфликт: {ex.Message}");
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении занятия");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // DELETE api/schedule/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            try
            {
                _logger.LogInformation($"Удаление занятия ID {id}");

                var deleted = await _lessonService.DeleteLessonAsync(id);

                if (!deleted)
                {
                    return NotFound(new { error = $"Занятие с ID {id} не найдено" });
                }

                return Ok(new { success = true, message = "Занятие успешно удалено", id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении занятия ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // DELETE api/schedule/group/{groupId}
        [HttpDelete("group/{groupId}")]
        public async Task<IActionResult> DeleteAllLessonsForGroup(int groupId)
        {
            try
            {
                _logger.LogInformation($"Удаление всех занятий для группы ID {groupId}");

                var count = await _lessonService.DeleteLessonsByGroupIdAsync(groupId);

                return Ok(new
                {
                    message = $"Удалено {count} занятий для группы",
                    count = count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении занятий для группы ID {groupId}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // PUT api/schedule/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonDto lessonDto)
        {
            try
            {
                _logger.LogInformation($"Обновление занятия ID {id}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedLesson = await _lessonService.UpdateLessonAsync(id, lessonDto);

                if (updatedLesson == null)
                {
                    return NotFound(new { error = $"Занятие с ID {id} не найдено" });
                }

                return Ok(updatedLesson);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Конфликт: {ex.Message}");
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении занятия ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        // GET api/schedule/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            try
            {
                _logger.LogInformation($"Получение занятия ID {id}");

                var lesson = await _lessonService.GetLessonByIdAsync(id);

                if (lesson == null)
                {
                    return NotFound(new { error = $"Занятие с ID {id} не найдено" });
                }

                return Ok(lesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении занятия ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }
    }
}