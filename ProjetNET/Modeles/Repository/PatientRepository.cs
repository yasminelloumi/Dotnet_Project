using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetNET.Modeles.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly Context _context;

        public PatientRepository(Context context)
        {
            _context = context;
        }

        // Add a new patient
        public async Task<Patient> Add(Patient patient)
        {
            var result = await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        
        public async Task<List<Patient>> GetAll()
        {
            return await _context.Patients.ToListAsync();
        }

        // Retrieve a patient by ID
        public async Task<Patient> GetById(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        // Update an existing patient
        public async Task Update(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        //hist
        public async Task<List<Medicament>> GetMedicamentsByPatientId(int patientId)
        {
            // Récupérez le patient avec ses ordonnances et leurs médicaments
            var patient = await _context.Patients
                .Include(p => p.Ordonnances)
                .ThenInclude(o => o.Medicaments)
                .FirstOrDefaultAsync(p => p.ID == patientId);

            if (patient == null)
            {
                return null; // Patient introuvable
            }

            // Récupérez tous les médicaments via les ordonnances
            return patient.Ordonnances
                          .SelectMany(o => o.Medicaments)
                          .Distinct()
                          .ToList();
        }

    }
}
