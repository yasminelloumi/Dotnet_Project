using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles.Repository;
using ProjetNET.Modeles;

namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandesController : ControllerBase
    {
        private readonly IDemandeRepository _demandeRepository;

        public DemandesController(IDemandeRepository demandeRepository)
        {
            _demandeRepository = demandeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDemandes()
        {
            var demandes = await _demandeRepository.GetAllDemandesAsync();
            return Ok(demandes);
        }

        [HttpPost]
        public async Task<IActionResult> AjouterDemande([FromBody] DemandeAchat demande)
        {
            await _demandeRepository.AjouterDemandeAsync(demande);
            return Ok("Demande ajoutée.");
        }

        [HttpPost("confirmer/{id}")]
        public async Task<IActionResult> ConfirmerAchat(int id)
        {
            await _demandeRepository.ConfirmerAchatAsync(id);
            return Ok("Achat confirmé.");
        }
    }
}
