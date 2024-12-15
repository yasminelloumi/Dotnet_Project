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

        // Create Ordonnance
        public async Task<OrdonnanceResponseDTO> CreateOrdonnanceAsync(CreateOrdonnanceDTO dto)
        {
            var patient = await context.Patients.FindAsync(dto.PatientId);
            if (patient == null)
            {
                throw new ArgumentException("Patient introuvable.");
            }

            var medecin = await context.Medecins
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.User.UserName == dto.MedecinName);

            if (medecin == null)
            {
                throw new ArgumentException("Médecin introuvable.");
            }

            var ordonnance = new Ordonnance
            {
                PatientId = patient.ID,
                MedecinId = medecin.Id,
                MedicamentOrdonnances = new List<MedicamentOrdonnance>()
            };

            var medicaments = new List<Medicament>();

            foreach (var medicamentDto in dto.Medicaments)
            {
                var medicament = await context.Medicaments.FindAsync(medicamentDto.MedicamentId);
                if (medicament == null)
                {
                    throw new ArgumentException($"Médicament avec l'ID {medicamentDto.MedicamentId} introuvable.");
                }

                var medicamentOrdonnance = new MedicamentOrdonnance
                {
                    IDMedicament = medicament.Id,
                    Quantite = medicamentDto.Quantite,
                    Medicament = medicament // Assurez-vous d'ajouter ceci
                };

                ordonnance.MedicamentOrdonnances.Add(medicamentOrdonnance);
                medicaments.Add(medicament);
            }

            await context.Ordonnances.AddAsync(ordonnance);
            await context.SaveChangesAsync();

            // Créer un historique avec les médicaments et leurs quantités
            var historique = new OrdonnanceHistorique
            {
                PatientName = patient.NamePatient,
                MedecinName = medecin.User.UserName,
                Medicaments = ordonnance.MedicamentOrdonnances.Select(mo => new MedicamentHistoriqueDTO
                {
                    MedicamentName = mo.Medicament.Name,
                    Quantite = mo.Quantite
                }).ToList(),
                CreationDate = DateTime.UtcNow
            };


            context.OrdonnanceHistoriques.Add(historique);
            await context.SaveChangesAsync();

            // Retourner la réponse DTO
            return new OrdonnanceResponseDTO
            {
                Id = ordonnance.Id,
                MedecinName = historique.MedecinName,
                PatientName = historique.PatientName,
                Medicaments = ordonnance.MedicamentOrdonnances.Select(mo => new MedicamentQuantityDTO
                {
                    MedicamentId = mo.Medicament.Id,
                    Quantite = mo.Quantite
                }).ToList()
            };
        }

        // Get Ordonnance by ID
        public async Task<OrdonnanceResponseDTO> GetOrdonnanceAsDTO(int id)
        {
            var ordonnance = await context.Ordonnances
                .Include(o => o.Medecin)
                    .ThenInclude(m => m.User)
                .Include(o => o.Patient)
                .Include(o => o.MedicamentOrdonnances)
                    .ThenInclude(mo => mo.Medicament)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordonnance == null)
            {
                return null;
            }

            var responseDto = new OrdonnanceResponseDTO
            {
                Id = ordonnance.Id,
                MedecinName = ordonnance.Medecin.User.UserName,
                PatientName = ordonnance.Patient.NamePatient,
                Medicaments = ordonnance.MedicamentOrdonnances.Select(mo => new MedicamentQuantityDTO
                {
                    MedicamentId = mo.Medicament.Id,
                    Quantite = mo.Quantite
                }).ToList()
            };

            return responseDto;
        }

        // Get All Ordonnances
        public async Task<IEnumerable<OrdonnanceResponseDTO>> GetAllOrdonnances()
        {
            var ordonnances = await context.Ordonnances
                .Include(o => o.Medecin)
                    .ThenInclude(m => m.User)
                .Include(o => o.Patient)
                .Include(o => o.MedicamentOrdonnances)
                    .ThenInclude(mo => mo.Medicament)
                .ToListAsync();

            return ordonnances.Select(o => new OrdonnanceResponseDTO
            {
                Id = o.Id,
                MedecinName = o.Medecin.User.UserName,
                PatientName = o.Patient.NamePatient,
                Medicaments = o.MedicamentOrdonnances.Select(mo => new MedicamentQuantityDTO
                {
                    MedicamentId = mo.Medicament.Id,
                    Quantite = mo.Quantite
                }).ToList()
            });
        }

        // Delete Ordonnance
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

        // Update Ordonnance
        public async Task<Ordonnance> UpdateOrdonnanceAsync(int id, UpdateOrdonnanceDTO dto)
        {
            // Retrieve the existing ordonnance
            var ordonnance = await context.Ordonnances
                .Include(o => o.MedicamentOrdonnances)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordonnance == null)
            {
                throw new ArgumentException("Ordonnance introuvable.");
            }

            // Retrieve the patient and validate
            var patient = await context.Patients.FindAsync(dto.PatientId);
            if (patient == null)
            {
                throw new ArgumentException("Patient introuvable.");
            }

            // Retrieve the doctor and validate
            var medecin = await context.Medecins
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.User.UserName == dto.MedecinName);

            if (medecin == null)
            {
                throw new ArgumentException("Médecin introuvable.");
            }

            // Update the ordonnance details
            ordonnance.PatientId = patient.ID;
            ordonnance.MedecinId = medecin.Id;

            // Clear existing medicaments and update with new ones
            ordonnance.MedicamentOrdonnances.Clear();

            foreach (var medicamentDto in dto.Medicaments)
            {
                var medicamentOrdonnance = new MedicamentOrdonnance
                {
                    IDMedicament = medicamentDto.MedicamentId,
                    Quantite = medicamentDto.Quantite
                };

                ordonnance.MedicamentOrdonnances.Add(medicamentOrdonnance);
            }

            // Save changes to the database
            await context.SaveChangesAsync();

            return ordonnance;
        }


        // Search Ordonnances by Medecin or Patient
        public async Task<IEnumerable<OrdonnanceResponseDTO>> SearchOrdonnances(string medecinId, int? patientId)
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

            var ordonnances = await query
                .Include(o => o.Medecin)
                    .ThenInclude(m => m.User)
                .Include(o => o.Patient)
                .Include(o => o.MedicamentOrdonnances)
                    .ThenInclude(mo => mo.Medicament)
                .ToListAsync();

            return ordonnances.Select(o => new OrdonnanceResponseDTO
            {
                Id = o.Id,
                MedecinName = o.Medecin.User.UserName,
                PatientName = o.Patient.NamePatient,
                Medicaments = o.MedicamentOrdonnances.Select(mo => new MedicamentQuantityDTO
                {
                    MedicamentId = mo.Medicament.Id,
                    Quantite = mo.Quantite
                }).ToList()
            });
        }

        // Get Ordonnance History
        public async Task<IEnumerable<OrdonnanceHistorique>> GetOrdonnanceHistory()
        {
            return await context.OrdonnanceHistoriques.ToListAsync();
        }
    }
}
