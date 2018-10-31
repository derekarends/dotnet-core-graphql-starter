using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.User
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> FindAsync();
        Task<UserModel> FindByIdAsync(string id);
        Task<UserModel> FindByEmailAsync(string email);
        Task<UserModel> InsertAsync(UserModel user);
        Task<bool> UpdateAsync(UserModel user);
        Task<bool> DeleteAsync(string id);
    }
}