using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles;
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

        
        [HttpPost("verifier-disponibilite")]
        public async Task ActionDemandeAchat(DemandeAchat demandeAchat)
        {
            bool isAvailable = await fournisseurRepository.VerifierDisponibiliteMedicament(demandeAchat);

            if (isAvailable)
            {
                Console.WriteLine("Le médicament est disponible en stock.");
            }
            else
            {
                Console.WriteLine("Le médicament n'est pas disponible ou la quantité demandée dépasse le stock.");
            }
        }

        [HttpPost("traiter")]
        public async Task<IList<string>> TraiterDemandesAchat([FromBody] List<DemandeAchat> demandesAchat)
        {
            if (demandesAchat == null || !demandesAchat.Any())
            {
                throw new Exception("La liste des demandes d'achat est vide.");
            }
            return await fournisseurRepository.TraitementDemandeAchat(demandesAchat);
        }

    }
}
