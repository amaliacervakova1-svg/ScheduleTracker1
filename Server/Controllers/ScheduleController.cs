using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _service;

        public ScheduleController(IScheduleService service)
        {
            _service = service;
        }

        // Посмотреть расписание группы на день
        [HttpGet]
        public async Task<IActionResult> GetSchedule(string group, int day, bool numerator)
        {
            if (!Enum.IsDefined(typeof(DayOfWeek), day))
                return BadRequest("Неверный день недели (0-6, где 0 = Воскресенье, 1 = Понедельник и т.д.)");

            var schedule = await _service.GetScheduleAsync(group, (DayOfWeek)day, numerator);
            return Ok(schedule);
        }
        // Посмотреть всё расписание
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var all = await _service.GetAllSchedulesAsync();
            return Ok(all);
        }

        // Добавить пару
        [HttpPost]
        public async Task<IActionResult> AddLesson([FromBody] Lesson lesson)
        {
            try
            {
                await _service.AddLessonAsync(lesson);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Удалить пару
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            try
            {
                await _service.DeleteLessonAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}