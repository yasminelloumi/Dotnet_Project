using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles.Repository;
using System.Threading.Tasks;

namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FournisseurController : ControllerBase
    {
        private readonly IFournisseurRepository fournisseurRepository;
        private readonly IMedicamentRepository medicamentRepository;

        public FournisseurController(IFournisseurRepository fournisseurRepository, IMedicamentRepository medicamentRepository)
        {
            this.fournisseurRepository = fournisseurRepository;
            this.medicamentRepository = medicamentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Fournisseurs = await fournisseurRepository.GetAll();
            return Ok(Fournisseurs);
        }

        // Méthode automatique pour envoyer une demande de réapprovisionnement
        [HttpPost("envoyer-automatique/{medicamentId}")]
        public async Task<IActionResult> EnvoyerAutomatique(int medicamentId)
        {
            var medicament = await medicamentRepository.GetById(medicamentId);
            if (medicament == null)
            {
                return NotFound($"Medicament avec ID {medicamentId} introuvable.");
            }

            if (medicament.QttStock <= 20)
            {
                const int quantiteAutomatique = 50;
                await fournisseurRepository.EnvoyerNotification(medicamentId, quantiteAutomatique);
                return Ok($"Demande automatique de {quantiteAutomatique} unités envoyée au fournisseur.");
            }

            return Ok("Stock suffisant. Pas besoin de demande au fournisseur.");
        }

        // Méthode manuelle pour envoyer une demande au fournisseur
        [HttpPost("envoyer-manuellement")]
        public async Task<IActionResult> EnvoyerManuellement(int medicamentId, int quantiteDemandee)
        {
            var medicament = await medicamentRepository.GetById(medicamentId);
            if (medicament == null)
            {
                return NotFound($"Medicament avec ID {medicamentId} introuvable.");
            }

            await fournisseurRepository.Envoyer(medicamentId, quantiteDemandee);
            return Ok($"Demande de {quantiteDemandee} unités du médicament avec ID {medicamentId} envoyée au fournisseur.");
        }
    }
}
