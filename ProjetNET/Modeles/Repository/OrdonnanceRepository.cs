using Microsoft.EntityFrameworkCore;

namespace ProjetNET.Modeles.Repository
{
    public class OrdonnanceRepository : IOrdonnanceRepository
    {
        private readonly Context context;

        public OrdonnanceRepository(Context context)
        {
            this.context = context;
        }

        public async Task<Ordonnance> Add(Ordonnance ordonnance)
        {
            var result = await context.Ordonnances.AddAsync(ordonnance);
            await context.SaveChangesAsync();
            return result.Entity;
        }       

    
        public async Task<List<Ordonnance>> GetAll()
        {
            List<Ordonnance> ordonnances = await context.Ordonnances
                .Include(o => o.Patient) // Inclure les détails du patient
                .Include(o => o.Medecin) // Inclure les détails du médecin
                .Include(o => o.Medicaments) // Inclure les médicaments associés
                .ToListAsync();
            return ordonnances;
        }

    
        public async Task<Ordonnance> GetById(int id)
        {
            Ordonnance ordonnance = await context.Ordonnances
                .Include(o => o.Patient)
                .Include(o => o.Medecin)
                .Include(o => o.Medicaments)
                .FirstOrDefaultAsync(o => o.IDOrdonnance == id);
            return ordonnance;
        }

        // Mettre à jour une ordonnance existante
        public async Task Update(Ordonnance ordonnance)
        {
            var existingOrdonnance = await context.Ordonnances.FindAsync(ordonnance.IDOrdonnance);
            if (existingOrdonnance != null)
            {
                existingOrdonnance.Date = ordonnance.Date;
                existingOrdonnance.IDPatient = ordonnance.IDPatient;
                existingOrdonnance.IDMedecin = ordonnance.IDMedecin;
                existingOrdonnance.Medicaments = ordonnance.Medicaments;

                context.Ordonnances.Update(existingOrdonnance);
                await context.SaveChangesAsync();
            }
        }
    }
}
