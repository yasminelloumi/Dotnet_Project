using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetNET.Modeles.Repository
{
    public interface IPatientRepository
    {
        // Retrieve all patients
        Task<List<Patient>> GetAll();

        // Retrieve a patient by ID
        Task<Patient> GetById(int id);


        // Add a new patient
        Task<Patient> Add(Patient patient);

        // Update an existing patient
        Task Update(Patient patient);

        // Delete a patient by ID
        // Task Delete(int id);

        //Task<List<Medicament>> GetMedicamentsByPatientId(int patientId);

        Task<List<Patient>> SearchPatients(string searchTerm);
    }

}

