using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles.Repository;
using ProjetNET.Modeles;
using ProjetNET.DTO;

namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdonnanceController : ControllerBase
    {
        private readonly IOrdonnanceRepository ordonnanceRepository;

        public OrdonnanceController(IOrdonnanceRepository ordonnanceRepository)
        {
            this.ordonnanceRepository = ordonnanceRepository;
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateOrdonnance([FromBody] CreateOrdonnanceDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await ordonnanceRepository.CreateOrdonnance(dto);
                return CreatedAtAction(nameof(GetOrdonnance), new { id = response.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdonnance(int id)
        {
            var ordonnance = await ordonnanceRepository.GetOrdonnanceAsDTO(id);
            if (ordonnance == null)
            {
                return NotFound("Ordonnance not found.");
            }
            return Ok(ordonnance);
        }

        // Méthode GET pour récupérer toutes les ordonnances
        [HttpGet]
        public async Task<IActionResult> GetAllOrdonnances()
        {
            var ordonnances = await ordonnanceRepository.GetAllOrdonnances();
            return Ok(ordonnances);
        }

        // Méthode DELETE pour supprimer une ordonnance
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdonnance(int id)
        {
            var success = await ordonnanceRepository.DeleteOrdonnance(id);
            if (!success)
            {
                return NotFound("Ordonnance not found.");
            }
            return NoContent();
        }

        // Méthode PUT pour mettre à jour une ordonnance
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrdonnance(int id, [FromBody] UpdateOrdonnanceDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await ordonnanceRepository.UpdateOrdonnance(id, dto);
                if (response == null)
                {
                    return NotFound("Ordonnance not found.");
                }
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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

            var ordonnances = await ordonnanceRepository.SearchOrdonnances(medecinId, patientId);

            if (ordonnances == null || !ordonnances.Any())
            {
                return NotFound("No ordonnances found matching the search criteria.");
            }

            return Ok(ordonnances);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetOrdonnanceHistory()
        {
            var history = await ordonnanceRepository.GetOrdonnanceHistory();
            return Ok(history);
        }






    }

}