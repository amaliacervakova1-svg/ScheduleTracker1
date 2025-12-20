using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation($"Попытка входа: {loginDto.Username}");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var isValid = await _authService.ValidateCredentialsAsync(
                    loginDto.Username,
                    loginDto.Password);

                if (isValid)
                {
                    return Ok(new AuthResponseDto
                    {
                        Success = true,
                        Message = "Аутентификация успешна"
                    });
                }
                else
                {
                    return Unauthorized(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Неверное имя пользователя или пароль"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при аутентификации пользователя {loginDto.Username}");
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Внутренняя ошибка сервера"
                });
            }
        }
    }
}