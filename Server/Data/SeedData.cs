using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Server.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Применяем все миграции (создаст таблицы если их нет)
            context.Database.Migrate();

            // === 1. СОЗДАЕМ АДМИНИСТРАТОРА (если его нет) ===
            if (!context.Admins.Any())
            {
                // Хэшируем пароль "12345" с помощью SHA256
                string password = "12345";
                string passwordHash = HashPassword(password);

                context.Admins.Add(new Admin
                {
                    Username = "admin",
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.UtcNow
                });

                context.SaveChanges();
                Console.WriteLine("✅ Создан администратор по умолчанию");
                Console.WriteLine("   Логин: admin");
                Console.WriteLine("   Пароль: 12345");
            }
            else
            {
                Console.WriteLine("ℹ️ Администратор уже существует в базе");
            }

            // === 2. ПРОВЕРЯЕМ, ЕСТЬ ЛИ УЖЕ ГРУППЫ ===
            if (context.Groups.Any())
            {
                Console.WriteLine("ℹ️ База уже заполнена данными, пропускаем создание тестовых данных");
                return;
            }

            Console.WriteLine("⏳ Начинаем заполнение базы тестовыми данными...");

            // === 3. СОЗДАЕМ ГРУППЫ ===
            var groupNames = new[] { "ИТ-21", "ИТ-22", "ИТ-23", "П-31", "П-32", "МК-15", "МК-16", "Э-41", "Б-11", "Т-51" };
            foreach (var name in groupNames)
                context.Groups.Add(new Group { Name = name });

            context.SaveChanges();
            Console.WriteLine($"✅ Создано {groupNames.Length} групп");

            // === 4. СОЗДАЕМ РАСПИСАНИЕ ===
            var rand = new Random();
            var subjects = new[] { "Программирование", "Базы данных", "Веб-технологии", "Математика", "Физика", "Английский", "ОСР", "Сети" };
            var teachers = new[] { "Петров А.В.", "Иванов И.И.", "Сидорова Е.К.", "Козлов В.П.", "Смирнова О.Н.", "Волков Д.С." };
            var rooms = new[] { "305", "412", "207", "101", "501", "310", "415" };

            int totalLessons = 0;

            foreach (var group in context.Groups.ToList())
            {
                // Для каждого дня недели (0-6, где 0 = Sunday)
                for (int day = 0; day < 7; day++)
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
                                DayOfWeek = (DayOfWeek)day, // Приводим int к DayOfWeek
                                IsNumerator = isNumerator,
                                Time = time,
                                Subject = subjects[rand.Next(subjects.Length)],
                                Teacher = teachers[rand.Next(teachers.Length)],
                                Room = rooms[rand.Next(rooms.Length)]
                            });

                            totalLessons++;
                        }
                    }
                }
            }

            context.SaveChanges();
            Console.WriteLine($"✅ Создано {totalLessons} занятий");
            Console.WriteLine("✅ База данных успешно инициализирована!");
        }

        // Метод для хэширования пароля (совпадает с методом в AuthService)
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}