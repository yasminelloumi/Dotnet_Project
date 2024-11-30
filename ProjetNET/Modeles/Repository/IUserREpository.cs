namespace ProjetNET.Modeles.Repository
{
    public interface IUserREpository
    {

        Task<User> AddUserAsync(User user);
        Task<Medecin> AddMedecinAsync(Medecin medecin);
        Task<Pharmacien> AddPharmacienAsync(Pharmacien pharmacien);
        Task<User> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserAsync(int id);

    }
    }

