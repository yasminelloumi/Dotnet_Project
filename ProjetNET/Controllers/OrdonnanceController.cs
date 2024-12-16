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
        public async Task<IActionResult> CreateOrdonnance([FromBody] CreateOrdonnanceDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Données invalides.");
            }

            try
            {
                // Appel à la méthode du repository pour créer l'ordonnance
                var ordonnanceResponse = await ordonnanceRepository.CreateOrdonnanceAsync(dto);

                // Retourner une réponse avec le DTO d'ordonnance
                return Ok(ordonnanceResponse);
            }
            catch (ArgumentException ex)
            {
                // Gestion des erreurs spécifiques
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Gestion des erreurs générales
                return StatusCode(500, $"Une erreur est survenue: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdonnance(int id)
        {
            try
            {
                // Fetch Ordonnance and its related MedicamentOrdonnances and Medicaments
                var ordonnance = await _context.Ordonnances
                    .Include(o => o.MedicamentOrdonnances)
                        .ThenInclude(mo => mo.Medicament) // Include Medicament in MedicamentOrdonnance
                    .Include(o => o.Patient) // Include Patient to access Patient's name
                    .Include(o => o.Medecin) // Include Medecin to access Medecin's user
                        .ThenInclude(m => m.User) // Include ApplicationUser (IdentityUser)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (ordonnance == null)
                {
                    return NotFound("Ordonnance not found.");
                }

                // Map Ordonnance to OrdonnanceDTO
                var ordonnanceDto = new OrdonnanceDTO
                {
                    Id = ordonnance.Id,
                    // Access the UserName from the Medecin's User (ApplicationUser)
                    MedecinName = ordonnance.Medecin?.User?.UserName ?? "Unknown Medecin",
                    PatientName = ordonnance.Patient?.NamePatient ?? "Unknown Patient", // Use NamePatient for the patient's name
                    Medicaments = ordonnance.MedicamentOrdonnances
                        .Select(mo => mo.Medicament.Name) // Only take the name of the Medicament
                        .ToList()
                };

                // Return the DTO
                return Ok(ordonnanceDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // Méthode GET pour récupérer toutes les ordonnances
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

        // Méthode DELETE pour supprimer une ordonnance
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

        // Méthode PUT pour mettre à jour une ordonnance
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
                return Ok(response); // Ordonnance mise à jour avec succès
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message }); // Erreur liée à l'argument
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Méthode GET pour rechercher des ordonnances par des critères
        [HttpGet("search")]
        public async Task<IActionResult> SearchOrdonnances([FromQuery] string medecinId, [FromQuery] int? patientId)
        {
            // Vérifier si aucun critère n'est donné
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

                return Ok(ordonnances); // Ordonnances correspondant aux critères
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
