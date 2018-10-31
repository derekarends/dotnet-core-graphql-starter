using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Context;

namespace Core.User
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAsync();
        Task<UserModel> GetByIdAsync(ContextModel ctx, string id);
        Task<UserModel> GetByEmailAsync(string email);
        Task<UserModel> CreateAsync(UserModel user);
        Task<UserModel> AuthenticateAsync(string email, string password);
        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        Task ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string userId, string newPassword);
        Task DeleteAsync(string id);
    }
}