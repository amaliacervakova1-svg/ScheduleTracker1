using Microsoft.EntityFrameworkCore;
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

        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Lesson>> GetScheduleAsync(string groupName, DayOfWeek dayOfWeek, bool isNumerator)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
            if (group == null) return new List<Lesson>();

            return await _context.Lessons
                .Where(l => l.GroupId == group.Id &&
                            l.DayOfWeek == dayOfWeek &&
                            l.IsNumerator == isNumerator)
                .OrderBy(l => l.Time)
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetAllSchedulesAsync()
        {
            return await _context.Lessons.Include(l => l.Group).ToListAsync();
        }

        public async Task AddLessonAsync(Lesson lesson)
        {
            var group = await _context.Groups.FindAsync(lesson.GroupId);
            if (group == null)
                throw new Exception("Группа не найдена");

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return;

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
        }
    }
}