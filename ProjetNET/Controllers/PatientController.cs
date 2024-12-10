using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles;
using ProjetNET.Modeles.Repository;
using System;
using System.Threading.Tasks;

namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        // GET: api/Patient
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientRepository.GetAll();
            return Ok(patients);
        }

        // GET: api/Patient/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientRepository.GetById(id);
            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            // Retourner le patient avec l'historique des médicaments
            return Ok(new
            {
                patient.ID,
                patient.NamePatient,
                patient.DateOfBirth,
                patient.MedicalHistory,
            });
        }

        // POST: api/Patient
        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            var createdPatient = await _patientRepository.Add(patient);
            if (createdPatient == null)
            {
                return BadRequest("Problem occurred while creating patient.");
            }

            return CreatedAtAction(nameof(GetById), new { id = createdPatient.ID }, createdPatient);
        }

        // PUT: api/Patient/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Patient patient)
        {
            // Set the incoming object's ID to the route ID
            patient.ID = id;

            if (id != patient.ID)
            {
                return BadRequest($"Patient ID mismatch. Route ID: {id}, Patient ID: {patient.ID}");
            }

            var existingPatient = await _patientRepository.GetById(id);
            if (existingPatient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            // Update the entity
            existingPatient.NamePatient = patient.NamePatient;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.MedicalHistory = patient.MedicalHistory;

            try
            {
                await _patientRepository.Update(existingPatient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent(); // Update successful
        }

        // DELETE: api/Patient/{id}
        // [HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //  var patient = await _patientRepository.GetById(id);
        //if (patient == null)
        //{
        //  return NotFound($"Patient with ID {id} not found.");
        //}

        //await _patientRepository.Delete(id);
        //return NoContent(); // Indicate the deletion was successful.
        //}

        [HttpGet("GetMedicaments/{patientId}")]
        public IActionResult GetMedicaments(int patientId)
        {
            var medicaments = _patientRepository.GetMedicamentsByPatientId(patientId);

            if (medicaments == null || !medicaments.Any())
            {
                return NotFound("No medicaments found for this patient.");
            }

            return Ok(medicaments);
        }
    }
}
