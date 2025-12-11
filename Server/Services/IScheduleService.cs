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
        Task AddLessonAsync(Lesson lesson);
        Task DeleteLessonAsync(int id);
    }
}
