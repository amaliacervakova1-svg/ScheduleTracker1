using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Linq;

namespace Server.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (context.Groups.Any())
                return; // уже заполняли

            // === ГРУППЫ ===
            var groupNames = new[] { "ИТ-21", "ИТ-22", "ИТ-23", "П-31", "П-32", "МК-15", "МК-16", "Э-41", "Б-11", "Т-51" };
            foreach (var name in groupNames)
                context.Groups.Add(new Group { Name = name });
            context.SaveChanges();

            // === РАСПИСАНИЕ ===
            var rand = new Random();
            var subjects = new[] { "Программирование", "Базы данных", "Веб-технологии", "Математика", "Физика", "Английский", "ОСР", "Сети" };
            var teachers = new[] { "Петров А.В.", "Иванов И.И.", "Сидорова Е.К.", "Козлов В.П.", "Смирнова О.Н.", "Волков Д.С." };
            var rooms = new[] { "305", "412", "207", "101", "501", "310", "415" };

            foreach (var group in context.Groups.ToList())
            {
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    for (int week = 0; week < 2; week++) // числитель и знаменатель
                    {
                        bool isNumerator = week == 0;
                        int pairsCount = rand.Next(3, 6); // 3–5 пар в день

                        for (int i = 0; i < pairsCount; i++)
                        {
                            string time = i switch
                            {
                                0 => "08:30–10:05",
                                1 => "10:15–11:50",
                                2 => "12:00–13:35",
                                3 => "14:00–15:35",
                                4 => "15:45–17:20",
                                _ => "17:30–19:05"
                            };

                            context.Lessons.Add(new Lesson
                            {
                                GroupId = group.Id,
                                DayOfWeek = day,
                                IsNumerator = isNumerator,
                                Time = time,
                                Subject = subjects[rand.Next(subjects.Length)],
                                Teacher = teachers[rand.Next(teachers.Length)],
                                Room = rooms[rand.Next(rooms.Length)]
                            });
                        }
                    }
                }
            }

            context.SaveChanges();
        }
    }
}