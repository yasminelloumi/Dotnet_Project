/*
using Microsoft.EntityFrameworkCore;

namespace ProjetNET.Modeles.Repository

{
    public class UserREpository : IUserREpository
    {
        private Context context;

        public UserREpository(Context context)
        {
            this.context = context;
        }
        // Ajouter un utilisateur générique
        public async Task<User> AddUserAsync(User user)
        {
            var result = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        // Ajouter un médecin
        public async Task<Medecin> AddMedecinAsync(Medecin medecin)
        {
            var result = await context.Users.AddAsync(medecin);
            await context.SaveChangesAsync();
            return (Medecin)result.Entity;
        }

        // Ajouter un pharmacien
        public async Task<Pharmacien> AddPharmacienAsync(Pharmacien pharmacien)
        {
            var result = await context.Users.AddAsync(pharmacien);
            await context.SaveChangesAsync();
            return (Pharmacien)result.Entity;
        }

        // Obtenir un utilisateur par ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        // Obtenir tous les utilisateurs
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        // Supprimer un utilisateur
        public async Task DeleteUserAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }


        }
    }

}


*/