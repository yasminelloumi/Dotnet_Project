using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles.Repository;
using ProjetNET.Modeles;

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

        // GET: api/Ordonnance
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ordonnances = await ordonnanceRepository.GetAll();
            return Ok(ordonnances);
        }

        // GET: api/Ordonnance/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ordonnance = await ordonnanceRepository.GetById(id);
            if (ordonnance == null)
            {
                return NotFound($"Ordonnance with ID {id} not found.");
            }
            return Ok(ordonnance);
        }

        // POST: api/Ordonnance
        [HttpPost]
        public async Task<IActionResult> Create(Ordonnance ordonnance)
        {
            if (ordonnance == null)
            {
                return BadRequest("Invalid ordonnance data.");
            }

            // Créez l'ordonnance dans la base de données
            await ordonnanceRepository.Add(ordonnance);

            // Récupérer le nom du patient et les noms des médicaments liés
            var patient = ordonnance.Patient; // Le patient est déjà lié à l'ordonnance
            var medicaments = ordonnance.Medicaments; // Liste des médicaments associés à l'ordonnance

            // Construire une réponse avec les noms du patient et des médicaments
            var result = new
            {
                OrdonnanceId = ordonnance.IDOrdonnance,
                Date = ordonnance.Date,
                PatientNom = patient?.NamePatient, 
                Medicaments = medicaments?.Select(m => m.Name) // Liste des noms des médicaments
            };

            return Ok(result);
        }

        // PUT: api/Ordonnance/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Ordonnance ordonnance)
        {
            // Ensure the ID in the route matches the ID in the model
            ordonnance.IDOrdonnance = id;

            var existingOrdonnance = await ordonnanceRepository.GetById(id);
            if (existingOrdonnance == null)
            {
                return NotFound($"Ordonnance with ID {id} not found.");
            }

            try
            {
                await ordonnanceRepository.Update(ordonnance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent(); // Update successful
        }

        
    }
}
