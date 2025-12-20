using System;
using System.Configuration;

namespace ScheduleClient
{
    public static class AppConfig
    {
        // Базовый URL сервера
        public static string BaseUrl => GetSetting("BaseUrl", "http://localhost:5032");

        // Пароль администратора
        public static string AdminPassword => GetSetting("AdminPassword", "12345");

        // Таймаут HTTP запросов
        public static TimeSpan HttpTimeout => TimeSpan.FromSeconds(GetIntSetting("HttpTimeoutSeconds", 30));

        // Высота статусной строки
        public static int StatusLabelHeight => GetIntSetting("StatusLabelHeight", 30);

        // Дни недели
        public static string[] DaysOfWeek => GetSetting("DaysOfWeek",
            "Понедельник,Вторник,Среда,Четверг,Пятница,Суббота,Воскресенье")
            .Split(',');

        // Временные периоды
        public static string[] TimePeriods => GetSetting("TimePeriods",
            "08:30–10:05;10:15–11:50;12:00–13:35;14:00–15:35;15:45–17:20;17:30–19:05;19:15–20:50")
            .Split(';');

        // Метод для получения строковой настройки
        private static string GetSetting(string key, string defaultValue = "")
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch
            {
                return defaultValue;
            }
        }

        // Метод для получения числовой настройки
        private static int GetIntSetting(string key, int defaultValue = 0)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return int.TryParse(value, out int result) ? result : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        // Метод для получения булевой настройки
        private static bool GetBoolSetting(string key, bool defaultValue = false)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return bool.TryParse(value, out bool result) ? result : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        // Логирование конфигурации при запуске
        public static void LogConfiguration()
        {
            Console.WriteLine("=== Конфигурация приложения ===");
            Console.WriteLine($"BaseUrl: {BaseUrl}");
            Console.WriteLine($"AdminPassword: {AdminPassword}");
            Console.WriteLine($"HttpTimeout: {HttpTimeout.TotalSeconds} сек");
            Console.WriteLine($"StatusLabelHeight: {StatusLabelHeight} пикс");
            Console.WriteLine($"DaysOfWeek: {string.Join(", ", DaysOfWeek)}");
            Console.WriteLine($"TimePeriods: {string.Join("; ", TimePeriods)}");
            Console.WriteLine("================================");
        }
    }
}