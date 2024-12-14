
using Microsoft.EntityFrameworkCore;

namespace ProjetNET.Modeles.Repository
{
    public class MedicamentRepository : IMedicamentRepository
    {
        private readonly Context context;
        public MedicamentRepository(Context context)
        {
            this.context = context;
            
        }

        public  async Task<Medicament> Add(Medicament medicament)
        {
            var result = await context.Medicaments.AddAsync(medicament);
            await context.SaveChangesAsync();
            return result.Entity
                ;
        }

        public async Task Delete(int id)
        {
            Medicament med = await context.Medicaments.FindAsync(id);
            context.Medicaments.Remove(med);
            await context.SaveChangesAsync();
        }

        public async Task<List<Medicament>> GetAll()
        {
            List<Medicament> medicaments = await context.Medicaments.ToListAsync();
            return medicaments;
        }

        public async Task<Medicament> GetById(int id)
        {
            Medicament med = await context.Medicaments.FindAsync(id);
            return med;
        }

        public async Task Update(Medicament medicament)
        {
            context.Medicaments.Update(medicament);
            await context.SaveChangesAsync();
        }

        // Méthode de recherche par nom ou ID
        public async Task<List<Medicament>> SearchMedicament(string searchTerm)
        {
            var query = context.Medicaments.AsQueryable();

            // Si le terme de recherche est un nombre, on suppose que c'est un ID
            if (int.TryParse(searchTerm, out int id))
            {
                // Recherche par ID
                query = query.Where(m => m.Id == id);
            }
            else
            {
                // Recherche par nom
                query = query.Where(m => m.Name.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

    }
}

