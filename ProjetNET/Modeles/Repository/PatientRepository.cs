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

        // Delete a patient by ID
       // public async Task Delete(int id)
        //{
          //  var patient = await _context.Patients.FindAsync(id);
            //if (patient != null)
            //{
              //  _context.Patients.Remove(patient);
                //await _context.SaveChangesAsync();
            //}
        //}

        // Retrieve all patients
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
    }
}
