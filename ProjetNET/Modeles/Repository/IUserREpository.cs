namespace ProjetNET.Modeles.Repository
{
    public interface IUserService
    {
        
            Task<User> AddUserAsync(User user);
            Task<User> DeleteUserAsync(int Id);
            Task<User> UpdateUserAsync(User user);
            Task<User> GetUserAsync(String name);
            List<User> List();

        }
    }

