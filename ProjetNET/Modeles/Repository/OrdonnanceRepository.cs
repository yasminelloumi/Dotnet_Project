using Microsoft.EntityFrameworkCore;
using ProjetNET.DTO;

namespace ProjetNET.Modeles.Repository
{
    public class OrdonnanceRepository : IOrdonnanceRepository
    {
        private readonly Context context;

        public OrdonnanceRepository(Context context)
        {
            this.context = context;
        }

        public async Task<OrdonnanceResponseDTO> CreateOrdonnance(CreateOrdonnanceDTO dto)
        {
            // Charger les entités nécessaires
            var medecin = await context.Medecins.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == dto.MedecinId);
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.ID == dto.PatientId);
            var medicaments = await context.Medicaments.Where(m => dto.MedicamentIds.Contains(m.Id)).ToListAsync();

            if (medecin == null || patient == null || !medicaments.Any())
            {
                throw new ArgumentException("Invalid data for creating ordonnance.");
            }

            // Créer l'ordonnance
            var ordonnance = new Ordonnance
            {
                Medecin = medecin,
                Patient = patient,
                Medicaments = medicaments
            };

            context.Ordonnances.Add(ordonnance);
            await context.SaveChangesAsync();

            // Retourner une réponse DTO
            return new OrdonnanceResponseDTO
            {
                Id = ordonnance.Id,
                MedecinName = medecin.User.UserName,
                PatientName = patient.NamePatient,
                Medicaments = medicaments.Select(m => m.Name).ToList()
            };
        }

        public async Task<OrdonnanceResponseDTO> GetOrdonnanceAsDTO(int id)
        {
            var ordonnance = await context.Ordonnances
                .Include(o => o.Medecin)
                    .ThenInclude(m => m.User) // Charger les données utilisateur du médecin
                .Include(o => o.Patient)
                .Include(o => o.Medicaments)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordonnance == null)
            {
                return null;
            }

            return new OrdonnanceResponseDTO
            {
                Id = ordonnance.Id,
                MedecinName = ordonnance.Medecin.User.UserName,
                PatientName = ordonnance.Patient.NamePatient,
                Medicaments = ordonnance.Medicaments.Select(m => m.Name).ToList()
            };
        }
        // Méthode pour récupérer toutes les ordonnances
        public async Task<IEnumerable<OrdonnanceResponseDTO>> GetAllOrdonnances()
        {
            var ordonnances = await context.Ordonnances
                .Include(o => o.Medecin)
                    .ThenInclude(m => m.User)
                .Include(o => o.Patient)
                .Include(o => o.Medicaments)
                .ToListAsync();

            return ordonnances.Select(o => new OrdonnanceResponseDTO
            {
                Id = o.Id,
                MedecinName = o.Medecin.User.UserName,
                PatientName = o.Patient.NamePatient,
                Medicaments = o.Medicaments.Select(m => m.Name).ToList()
            });
        }

        // Méthode pour supprimer une ordonnance
        public async Task<bool> DeleteOrdonnance(int id)
        {
            var ordonnance = await context.Ordonnances.FindAsync(id);
            if (ordonnance == null)
            {
                return false;
            }

            context.Ordonnances.Remove(ordonnance);
            await context.SaveChangesAsync();
            return true;
        }

        // Méthode pour mettre à jour une ordonnance
        public async Task<OrdonnanceResponseDTO> UpdateOrdonnance(int id, UpdateOrdonnanceDTO dto)
        {
            var ordonnance = await context.Ordonnances
                .Include(o => o.Medicaments)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordonnance == null)
            {
                return null;
            }

            // Charger les nouvelles données
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.ID == dto.PatientId);
            var medicaments = await context.Medicaments.Where(m => dto.MedicamentIds.Contains(m.Id)).ToListAsync();

            if (patient == null || !medicaments.Any())
            {
                throw new ArgumentException("Invalid data for updating ordonnance.");
            }

            // Mettre à jour les propriétés
            ordonnance.Patient = patient;
            ordonnance.Medicaments = medicaments;

            await context.SaveChangesAsync();

            return new OrdonnanceResponseDTO
            {
                Id = ordonnance.Id,
                MedecinName = ordonnance.Medecin.User.UserName,
                PatientName = ordonnance.Patient.NamePatient,
                Medicaments = medicaments.Select(m => m.Name).ToList()
            };
        }
        public async Task<List<Ordonnance>> SearchOrdonnances(string medecinId, int? patientId)
        {
            var query = context.Ordonnances.AsQueryable();

            // Recherche par MedecinId
            if (!string.IsNullOrEmpty(medecinId))
            {
                query = query.Where(o => o.MedecinId.Contains(medecinId));
            }

            // Recherche par PatientId
            if (patientId.HasValue)
            {
                query = query.Where(o => o.PatientId == patientId.Value);
            }

            // Inclure les entités associées, si nécessaire
            return await query
                        .Include(o => o.Patient)
                        .Include(o => o.Medecin)
                        .Include(o => o.Medicaments)
                        .ToListAsync();
        }
    }
    



}
