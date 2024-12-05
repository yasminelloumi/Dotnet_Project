
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetNET.DTO;
using ProjetNET.Modeles;
using ProjetNET.Repositories;
using System.Threading.Tasks;

namespace ProjetNET.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        


        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
     

        [HttpGet("{userId}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] ApplicationUser user, [FromQuery] string password)
        {
            var createdUser = await _userRepository.CreateUserAsync(user, password);
            return CreatedAtAction(nameof(GetUserById), new { userId = createdUser.Id }, createdUser);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<ApplicationUser>> UpdateUser(string userId, [FromBody] ApplicationUser user)
        {
            if (userId != user.Id) return BadRequest("User ID mismatch");
            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var success = await _userRepository.DeleteUserAsync(userId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{userId}/role/{roleName}")]
        public async Task<ActionResult> AssignRoleToUser(string userId, string roleName)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();
            await _userRepository.AssignRoleToUserAsync(user, roleName);
            return NoContent();
        }

        [HttpDelete("{userId}/role/{roleName}")]
        public async Task<ActionResult> RemoveRoleFromUser(string userId, string roleName)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();
            await _userRepository.RemoveRoleFromUserAsync(user, roleName);
            return NoContent();
        }

       

        
    }
}
