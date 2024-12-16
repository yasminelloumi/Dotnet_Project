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
            return await _context.Patients
                .Include(p => p.Ordonnances)
                    .ThenInclude(o => o.MedicamentOrdonnances)
                .ToListAsync();
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
        public async Task<Patient> GetPatientWithHistorique(int id)
        {
            return await _context.Patients
                .Include(p => p.Ordonnances)
                .ThenInclude(o => o.MedicamentOrdonnances)
                .ThenInclude(mo => mo.Medicament)
                .FirstOrDefaultAsync(p => p.ID == id);
        }
        //methode recherche
        public async Task<List<Patient>> SearchPatients(string searchTerm)
        {
            // Recherche par nom ou date de naissance
            var query = _context.Patients.AsQueryable();

            // Si le terme de recherche est un nombre, on suppose que c'est une date ou ID
            if (int.TryParse(searchTerm, out int id))
            {
                // Recherche par ID
                query = query.Where(p => p.ID == id);
            }
            else if (DateTime.TryParse(searchTerm, out DateTime birthDate))
            {
                // Recherche par date de naissance
                query = query.Where(p => p.DateOfBirth == birthDate);
            }
            else
            {
                // Recherche par nom
                query = query.Where(p => p.NamePatient.Contains(searchTerm));
            }

            return await query
                        .Include(p => p.Ordonnances)
                            .ThenInclude(o => o.MedicamentOrdonnances)
                        .ToListAsync();
        }

        public async Task<List<string>> GetMedicamentsByPatientId(int patientId)
        {
            var patient = await _context.Patients
                .Include(p => p.Ordonnances)
                .ThenInclude(o => o.MedicamentOrdonnances)
                .ThenInclude(mo => mo.Medicament)
                .FirstOrDefaultAsync(p => p.ID == patientId);

            if (patient == null)
            {
                return null;
            }

            return patient.Ordonnances
                          .SelectMany(o => o.MedicamentOrdonnances)
                          .Select(mo => mo.Medicament.Name)
                          .Distinct()
                          .ToList();
        }


    }
}
