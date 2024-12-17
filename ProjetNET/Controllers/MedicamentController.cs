using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.DTO;
using ProjetNET.Modeles;
using ProjetNET.Modeles.Repository;

namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentController : ControllerBase
    {
        private readonly IMedicamentRepository medicamentRepository;
       

        public MedicamentController(IMedicamentRepository medicamentRepository)
        {
            this.medicamentRepository = medicamentRepository;
           
        }


        // GET: api/Medicament
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicaments = await medicamentRepository.GetAll();
            return Ok(medicaments);
        }

        // GET: api/Medicament/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            var medicament = await medicamentRepository.GetById(id);
            if (medicament == null)
            {
                return NotFound($"Medicament with ID {id} not found.");
            }
            return Ok(medicament);
        }

        // POST: api/Medicament
        [HttpPost]
        public async Task<IActionResult> Create(Medicament medicament)
        {
            var med = await medicamentRepository.Add(medicament);
            if (med == null)
            {
                return BadRequest("Problem occurred while creating medicament.");
            }

            return CreatedAtAction(nameof(GetById), new { id = med.Id }, med);
        }

        // PUT: api/Medicament/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Medicament medicament)
        {
            // Set the incoming object's ID to the route ID
            medicament.Id = id;

            if (id != medicament.Id)
            {
                return BadRequest($"Medicament ID mismatch. Route ID: {id}, Medicament ID: {medicament.Id}");
            }

            var existingMedicament = await medicamentRepository.GetById(id);
            if (existingMedicament == null)
            {
                return NotFound($"Medicament with ID {id} not found.");
            }

            // Update the entity
            existingMedicament.Name = medicament.Name;
            existingMedicament.Description = medicament.Description;
            existingMedicament.Prix = medicament.Prix;
            existingMedicament.QttStock = medicament.QttStock;
         

            try
            {
                await medicamentRepository.Update(existingMedicament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent(); // Update successful
        }


        // DELETE: api/Medicament/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var medicament = await medicamentRepository.GetById(id);
            if (medicament == null)
            {
                return NotFound($"Medicament with ID {id} not found.");
            }

            await medicamentRepository.Delete(id);
            return NoContent(); // Indicate the deletion was successful.
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search term is required.");
            }

            // Recherche des médicaments
            var medicaments = await medicamentRepository.SearchMedicament(searchTerm);

            if (medicaments == null || !medicaments.Any())
            {
                return NotFound("No medicaments found matching the search term.");
            }

            var medicamentDTOs = medicaments.Select(m => new
            {
                m.Id,
                m.Name,
                m.Description,
                m.Prix,
                m.QttStock
            });

            return Ok(medicamentDTOs);
        }
        [HttpGet("seuil")]
        public async Task<IActionResult> GetMedicamentsSeuil()
        {
            var medicaments = await medicamentRepository.GetMedicamentsEnSeuilAsync();
            return Ok(medicaments);
        }

        [HttpPost("ajouter-stock/{id}")]
        public async Task<IActionResult> AjouterStock(int id, [FromBody] int quantite)
        {
            await medicamentRepository.AjouterStockMedicamentAsync(id, quantite);
            return Ok("Stock ajouté avec succès.");
        }

        // Endpoint pour récupérer les médicaments en seuil critique
        [HttpGet("en-seuil-pour-demande")]
        public async Task<IActionResult> GetMedicamentsEnSeuilPourDemande()
        {
            var medicaments = await medicamentRepository.GetMedicamentsEnSeuilPourDemandeAsync();
            return Ok(medicaments);
        }

        // Endpoint pour soumettre les demandes d'achat
        [HttpPost("envoyer-demande")]
        public async Task<IActionResult> EnvoyerDemande([FromBody] List<MedicamentDemandeDto> demandes)
        {
            if (demandes == null || !demandes.Any())
            {
                return BadRequest("Aucune demande reçue.");
            }

            var result = await medicamentRepository.AjouterDemandeMedicamentAsync(demandes);
            if (result)
                return Ok("La demande a été envoyée avec succès.");
            else
                return StatusCode(500, "Une erreur est survenue lors de l'envoi de la demande.");
        }




    }
}
