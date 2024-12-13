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
            // Load Patient, Medecin, and Medicaments
            var patient = await context.Patients.FindAsync(dto.PatientId);
            var medecin = await context.Medecins.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == dto.MedecinId);
            var medicaments = await context.Medicaments.Where(m => dto.MedicamentIds.Contains(m.Id)).ToListAsync();

            // Validate input data
            if (patient == null || medecin == null || !medicaments.Any())
            {
                throw new ArgumentException("Invalid Patient, Medecin, or Medicaments data.");
            }

            // Create the Ordonnance entity
            var ordonnance = new Ordonnance
            {
                Patient = patient,
                Medecin = medecin,
                Medicaments = medicaments
            };

            context.Ordonnances.Add(ordonnance);
            await context.SaveChangesAsync();

            // Save to OrdonnanceHistorique
            var historique = new OrdonnanceHistorique
            {
                PatientName = patient.NamePatient,
                MedecinName = medecin.User.UserName,
                MedicamentNames = medicaments.Select(m => m.Name).ToList(),
                CreationDate = DateTime.UtcNow
            };

            context.OrdonnanceHistoriques.Add(historique);
            await context.SaveChangesAsync();

            // Return the response DTO
            return new OrdonnanceResponseDTO
            {
                Id = ordonnance.Id,
                MedecinName = historique.MedecinName,
                PatientName = historique.PatientName,
                Medicaments = historique.MedicamentNames
            };
        }

        public async Task<OrdonnanceResponseDTO> GetOrdonnanceAsDTO(int id)
        {
            var ordonnance = await context.Ordonnances
                .Include(o => o.Medecin)
                    .ThenInclude(m => m.User)
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

        public async Task<OrdonnanceResponseDTO> UpdateOrdonnance(int id, UpdateOrdonnanceDTO dto)
        {
            var ordonnance = await context.Ordonnances
                .Include(o => o.Medicaments)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordonnance == null)
            {
                return null;
            }

            var patient = await context.Patients.FirstOrDefaultAsync(p => p.ID == dto.PatientId);
            var medicaments = await context.Medicaments.Where(m => dto.MedicamentIds.Contains(m.Id)).ToListAsync();

            if (patient == null || !medicaments.Any())
            {
                throw new ArgumentException("Invalid data for updating ordonnance.");
            }

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

            if (!string.IsNullOrEmpty(medecinId))
            {
                query = query.Where(o => o.MedecinId == medecinId);
            }

            if (patientId.HasValue)
            {
                query = query.Where(o => o.PatientId == patientId.Value);
            }

            return await query
                .Include(o => o.Patient)
                .Include(o => o.Medecin)
                .Include(o => o.Medicaments)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrdonnanceHistorique>> GetOrdonnanceHistory()
        {
            return await context.OrdonnanceHistoriques.ToListAsync();
        }

        Task<IEnumerable<OrdonnanceResponseDTO>> IOrdonnanceRepository.SearchOrdonnances(string medecinId, int? patientId)
        {
            throw new NotImplementedException();
        }
    }
}
