using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using System.Security.Cryptography;
using System.Text;

namespace Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            try
            {
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Username == username);

                if (admin == null)
                {
                    _logger.LogWarning($"Попытка входа с несуществующим пользователем: {username}");
                    return false;
                }

                // Простая проверка пароля (в будущем можно добавить хэширование)
                // Пока сравниваем как есть, но лучше использовать BCrypt или подобное
                bool isValid = admin.PasswordHash == HashPassword(password);

                if (isValid)
                {
                    admin.LastLoginAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Успешный вход: {username}");
                }
                else
                {
                    _logger.LogWarning($"Неверный пароль для пользователя: {username}");
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при проверке учетных данных для {username}");
                return false;
            }
        }

        public async Task<Admin?> GetAdminByUsernameAsync(string username)
        {
            return await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<bool> ChangePasswordAsync(string username, string oldPassword, string newPassword)
        {
            try
            {
                var admin = await GetAdminByUsernameAsync(username);
                if (admin == null) return false;

                if (!await ValidateCredentialsAsync(username, oldPassword))
                    return false;

                admin.PasswordHash = HashPassword(newPassword);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Пароль изменен для пользователя: {username}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при изменении пароля для {username}");
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Простое хэширование SHA256 (в продакшене лучше использовать BCrypt)
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Метод для инициализации первого администратора
        public async Task InitializeDefaultAdminAsync()
        {
            if (!await _context.Admins.AnyAsync())
            {
                var defaultAdmin = new Admin
                {
                    Username = "admin",
                    PasswordHash = HashPassword("12345"),
                    CreatedAt = DateTime.UtcNow
                };

                _context.Admins.Add(defaultAdmin);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Создан администратор по умолчанию: admin/12345");
            }
        }
    }
}