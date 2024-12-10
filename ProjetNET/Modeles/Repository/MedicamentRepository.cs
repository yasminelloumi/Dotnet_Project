
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

        public async Task<List<Medicament>> GetMedicamentsByPatientIdAsync(int patientId)
        {
            return await context.Ordonnances
                .Where(o => o.IDPatient == patientId)
                .SelectMany(o => o.Medicaments)
                .Distinct()
                .ToListAsync();
        }
    }
}
// add list Medicaments to context!!!!!!!! dont forget 
