using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            existingMedicament.DateExpiration = medicament.DateExpiration;

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
    }
}
