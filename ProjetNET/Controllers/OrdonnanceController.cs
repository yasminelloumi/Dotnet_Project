using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles.Repository;
using ProjetNET.Modeles;
using ProjetNET.DTO;
using Microsoft.EntityFrameworkCore;

namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdonnanceController : ControllerBase
    {
        private readonly Context _context;
        private readonly IOrdonnanceRepository ordonnanceRepository;

        public OrdonnanceController(Context context, IOrdonnanceRepository ordonnanceRepository)
        {
            _context = context;
            this.ordonnanceRepository = ordonnanceRepository;
        }


        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CreateOrdonnance([FromBody] CreateOrdonnanceDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Données invalides.");
            }

            try
            {
                // Ensure the patient exists
                var patient = await _context.Patients.FindAsync(dto.PatientId);
                if (patient == null)
                {
                    return BadRequest("Patient introuvable.");
                }

                // Ensure the doctor exists
                var medecin = await _context.Medecins
                    .Include(m => m.User)
                    .FirstOrDefaultAsync(m => m.User.UserName == dto.MedecinName);

                if (medecin == null)
                {
                    return BadRequest("Médecin introuvable.");
                }

                // Create the Ordonnance
                var ordonnance = new Ordonnance
                {
                    PatientId = patient.ID,
                    MedecinId = medecin.Id,
                    MedicamentOrdonnances = new List<MedicamentOrdonnance>()
                };

                // List to track messages for medications that can't be processed
                var failedMedications = new List<string>();

                // Process each medicament from the DTO
                foreach (var medicamentDTO in dto.Medicaments)
                {
                    var medicament = await _context.Medicaments.FindAsync(medicamentDTO.MedicamentId);
                    if (medicament == null)
                    {
                        failedMedications.Add($"Médicament avec l'ID {medicamentDTO.MedicamentId} introuvable.");
                        continue; // Skip this medication
                    }

                    // Check if the requested quantity is available
                    if (medicamentDTO.Quantite > medicament.QttStock)
                    {
                        failedMedications.Add($"La quantité de {medicament.Name} est insuffisante.");
                        continue; // Skip this medication
                    }

                    // Create MedicamentOrdonnance record for the valid medication
                    var medicamentOrdonnance = new MedicamentOrdonnance
                    {
                        IDMedicament = medicament.Id,
                        Quantite = medicamentDTO.Quantite,
                        Medicament = medicament
                    };

                    // Reduce the stock for the medication
                    medicament.QttStock -= medicamentDTO.Quantite;

                    // Add to the Ordonnance's list of MedicamentOrdonnances
                    ordonnance.MedicamentOrdonnances.Add(medicamentOrdonnance);
                }

                // If no medications could be processed, return a failure response
                if (ordonnance.MedicamentOrdonnances.Count == 0)
                {
                    return BadRequest(new { Message = "Aucun médicament valide n'a été trouvé pour l'ordonnance.", FailedMedications = failedMedications });
                }

                // Save the Ordonnance and related MedicamentOrdonnances
                await _context.Ordonnances.AddAsync(ordonnance);
                await _context.SaveChangesAsync();

                // Now that the Ordonnance is created, remove the MedicamentOrdonnances from the table
                foreach (var medicamentOrdonnance in ordonnance.MedicamentOrdonnances)
                {
                    _context.MedicamentOrdonnances.Remove(medicamentOrdonnance);
                }

                await _context.SaveChangesAsync();

                // Return a response with the successful ordonnance and the failed medications
                return Ok(new { Ordonnance = ordonnance, FailedMedications = failedMedications });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdonnance(int id)
        {
            try
            {
                var ordonnance = await _context.Ordonnances
                    .Include(o => o.MedicamentOrdonnances)
                        .ThenInclude(mo => mo.Medicament) 
                    .Include(o => o.Patient) 
                    .Include(o => o.Medecin)
                        .ThenInclude(m => m.User) 
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (ordonnance == null)
                {
                    return NotFound("Ordonnance not found.");
                }

                var ordonnanceDto = new OrdonnanceDTO
                {
                    Id = ordonnance.Id,
                    MedecinName = ordonnance.Medecin?.User?.UserName ?? "Unknown Medecin",
                    PatientName = ordonnance.Patient?.NamePatient ?? "Unknown Patient", 
                    Medicaments = ordonnance.MedicamentOrdonnances
                        .Select(mo => mo.Medicament.Name) 
                        .ToList()
                };

                return Ok(ordonnanceDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdonnances()
        {
            try
            {
                var ordonnances = await ordonnanceRepository.GetAllOrdonnances();
                if (ordonnances == null || !ordonnances.Any())
                {
                    return NotFound("No ordonnances found.");
                }
                return Ok(ordonnances);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdonnance(int id)
        {
            try
            {
                var success = await ordonnanceRepository.DeleteOrdonnance(id);
                if (!success)
                {
                    return NotFound("Ordonnance not found.");
                }
                return NoContent(); // HTTP 204 No Content - suppression réussie
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrdonnance(int id, [FromBody] UpdateOrdonnanceDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Si le modèle est invalide, renvoyer BadRequest
            }

            try
            {
                var response = await ordonnanceRepository.UpdateOrdonnanceAsync(id, dto);
                if (response == null)
                {
                    return NotFound("Ordonnance not found.");
                }
                return Ok(response); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchOrdonnances([FromQuery] string medecinId, [FromQuery] int? patientId)
        {
            if (string.IsNullOrEmpty(medecinId) && patientId == null)
            {
                return BadRequest("At least one search criterion (MedecinId or PatientId) must be provided.");
            }

            try
            {
                var ordonnances = await ordonnanceRepository.SearchOrdonnances(medecinId, patientId);

                if (ordonnances == null || !ordonnances.Any())
                {
                    return NotFound("No ordonnances found matching the search criteria.");
                }

                return Ok(ordonnances); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

         [HttpGet("historique")]
    public async Task<IActionResult> GetHistorique()
    {
        try
        {
            var historique = await ordonnanceRepository.GetHistoriqueAsync();

            if (historique == null || !historique.Any())
            {
                return NotFound(new { Message = "Aucun historique trouvé." });
            }

            return Ok(historique);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Une erreur s'est produite.", Details = ex.Message });
        }
    }
    }
}
