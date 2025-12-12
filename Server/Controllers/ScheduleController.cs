using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _service;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(IScheduleService service, ILogger<ScheduleController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // Посмотреть расписание группы на день
        [HttpGet]
        public async Task<IActionResult> GetSchedule(
            [Required] string group,
            [Range(0, 6)] int day,
            bool numerator)
        {
            try
            {
                _logger.LogInformation($"Запрос расписания: группа={group}, день={day}, числитель={numerator}");

                if (!Enum.IsDefined(typeof(DayOfWeek), day))
                    return BadRequest(new { error = "Неверный день недели (0-6, где 0 = Воскресенье, 1 = Понедельник и т.д.)" });

                var schedule = await _service.GetScheduleAsync(group, (DayOfWeek)day, numerator);

                // Возвращаем чистый список без обертки
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении расписания для группы {group}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        // Посмотреть всё расписание
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSchedules()
        {
            try
            {
                var all = await _service.GetAllSchedulesAsync();
                return Ok(all);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении всех расписаний");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        // Добавить пару
        [HttpPost]
        public async Task<IActionResult> AddLesson([FromBody] CreateLessonDto lessonDto)
        {
            try
            {
                _logger.LogInformation($"Добавление занятия для группы ID {lessonDto.GroupId}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Создаем Lesson из DTO
                var lesson = new Lesson
                {
                    GroupId = lessonDto.GroupId,
                    DayOfWeek = (DayOfWeek)lessonDto.DayOfWeek,
                    IsNumerator = lessonDto.IsNumerator,
                    Time = lessonDto.Time,
                    Subject = lessonDto.Subject,
                    Teacher = lessonDto.Teacher,
                    Room = lessonDto.Room
                };

                var addedLesson = await _service.AddLessonAsync(lesson);
                return Ok(addedLesson);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Ошибка аргумента при добавлении занятия: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Ошибка операции при добавлении занятия: {ex.Message}");
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении занятия для группы ID {lessonDto.GroupId}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        // Удалить пару
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            try
            {
                _logger.LogInformation($"Удаление занятия ID {id}");

                var deleted = await _service.DeleteLessonAsync(id);

                if (!deleted)
                    return NotFound(new { error = $"Занятие с ID {id} не найдено" });

                return Ok(new { success = true, message = "Занятие успешно удалено", id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении занятия ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }

        // Удалить все занятия группы
        [HttpDelete("group/{groupId}")]
        public async Task<IActionResult> DeleteAllLessonsForGroup(int groupId)
        {
            try
            {
                _logger.LogInformation($"Удаление всех занятий для группы ID {groupId}");

                var count = await _service.DeleteAllLessonsForGroupAsync(groupId);

                return Ok(new
                {
                    message = $"Удалено {count} занятий для группы",
                    count = count
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Ошибка аргумента при удалении занятий группы: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении занятий для группы ID {groupId}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера", details = ex.Message });
            }
        }
    }
}