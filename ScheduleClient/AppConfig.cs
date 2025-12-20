using System;
using System.Configuration;

namespace ScheduleClient
{
    public static class AppConfig
    {
        // Метод для получения дней недели из конфигурации
        public static string[] GetDaysOfWeek()
        {
            try
            {
                // Пытаемся получить настройку из App.config
                string daysConfig = ConfigurationManager.AppSettings["DaysOfWeek"];

                // Если настройка не найдена, используем значения по умолчанию
                if (string.IsNullOrEmpty(daysConfig))
                {
                    Console.WriteLine("Настройка DaysOfWeek не найдена, используются значения по умолчанию");
                    return new string[]
                    {
                        "Понедельник", "Вторник", "Среда",
                        "Четверг", "Пятница", "Суббота", "Воскресенье"
                    };
                }

                // Разделяем строку по запятым
                string[] days = daysConfig.Split(',');

                // Убираем лишние пробелы
                for (int i = 0; i < days.Length; i++)
                {
                    days[i] = days[i].Trim();
                }

                Console.WriteLine($"Загружены дни недели из конфигурации: {string.Join(", ", days)}");
                return days;
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, используем значения по умолчанию
                Console.WriteLine($"Ошибка при загрузке DaysOfWeek: {ex.Message}");
                return new string[]
                {
                    "Понедельник", "Вторник", "Среда",
                    "Четверг", "Пятница", "Суббота", "Воскресенье"
                };
            }
        }
    }
}