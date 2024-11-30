using Microsoft.AspNetCore.Mvc;
using ProjetNET.Modeles.Repository;
using ProjetNET.Modeles;

namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserREpository _userRepository;

        public UserController(UserREpository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }
            return Ok(user);
        }

        // POST: api/User/Medecin
        [HttpPost("Medecin")]
        public async Task<ActionResult<Medecin>> AddMedecin([FromBody] Medecin medecin)
        {
            if (medecin == null)
            {
                return BadRequest(new { Message = "Invalid Medecin data." });
            }

            var createdMedecin = await _userRepository.AddMedecinAsync(medecin);
            return CreatedAtAction(nameof(GetUserById), new { id = createdMedecin.Id }, createdMedecin);
        }

        // POST: api/User/Pharmacien
        [HttpPost("Pharmacien")]
        public async Task<ActionResult<Pharmacien>> AddPharmacien([FromBody] Pharmacien pharmacien)
        {
            if (pharmacien == null)
            {
                return BadRequest(new { Message = "Invalid Pharmacien data." });
            }

            var createdPharmacien = await _userRepository.AddPharmacienAsync(pharmacien);
            return CreatedAtAction(nameof(GetUserById), new { id = createdPharmacien.Id }, createdPharmacien);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "Invalid User data." });
            }

            var createdUser = await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
