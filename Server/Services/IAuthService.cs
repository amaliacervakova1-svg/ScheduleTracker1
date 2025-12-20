using Server.Models;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<Admin?> GetAdminByUsernameAsync(string username);
        Task<bool> ChangePasswordAsync(string username, string oldPassword, string newPassword);
    }
}