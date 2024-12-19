using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles.Repository;
using ProjetNET.Modeles;
using ProjetNET.DTO;

namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentOrdonnanceController : ControllerBase
    {
        private readonly IMedicamentOrdonnanceRepository _medicamentOrdonnanceRepository;

        public MedicamentOrdonnanceController(IMedicamentOrdonnanceRepository medicamentOrdonnanceRepository)
        {
            _medicamentOrdonnanceRepository = medicamentOrdonnanceRepository;
        }

    
        [HttpPost("verify-medication")]
        public async Task<IActionResult> VerifierDisponibiliteMedicament([FromBody] MedicamentOrdonnance medicamentOrdonnance)
        {
            if (medicamentOrdonnance == null)
            {
                return BadRequest("Demande de médicament invalide.");
            }

            try
            {
                var isAvailable = await _medicamentOrdonnanceRepository.VerifierDisponibiliteMedicament(medicamentOrdonnance);
                return Ok(new { Available = isAvailable });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("process-ordonnances")]
        public async Task<IActionResult> TraitementDemandeOrdonnance([FromBody] List<MedicamentOrdonnance> medicamentOrdonnances)
        {
            if (medicamentOrdonnances == null || medicamentOrdonnances.Count == 0)
            {
                return BadRequest("La demande de médicaments est vide.");
            }

            try
            {
                var results = await _medicamentOrdonnanceRepository.TraitementDemandeOrdonnance(medicamentOrdonnances);
                return Ok(new { Results = results });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
