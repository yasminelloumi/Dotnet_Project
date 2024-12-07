using System.Collections.Generic;
using System.Threading.Tasks;
using ProjetNET.Modeles;

namespace ProjetNET.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> CreateUserAsync(string userName, string email, string password);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<IList<ApplicationUser>> GetAllUsersAsync();
        Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName);
        Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string roleName);
        Task<string> AuthenticateUserAsync(string username, string password);

    }
}