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
            var newOrdonnance = await ordonnanceRepository.Add(ordonnance);
            if (newOrdonnance == null)
            {
                return BadRequest("Problem occurred while creating ordonnance.");
            }

            return CreatedAtAction(nameof(GetById), new { id = newOrdonnance.IDOrdonnance }, newOrdonnance);
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
