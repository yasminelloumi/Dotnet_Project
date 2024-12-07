
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
        [HttpGet("")]
        public async Task<ActionResult<ApplicationUser>> GetAllUsers(string userId)
        {
            var user = await _userRepository.GetAllUsersAsync();
            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Call the repository method to create the user
                var createdUser = await _userRepository.CreateUserAsync(model.UserName, model.Email, model.Password);

                // Return the created user with a 201 Created response
                return CreatedAtAction(nameof(GetUserById), new { userId = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpPut("{userId}")]
        public async Task<ActionResult<ApplicationUser>> UpdateUser(string userId, [FromBody] ApplicationUser user)
        {
            if (userId != user.Id) return BadRequest("User ID mismatch");
            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteUser(string email)
        {
            var success = await _userRepository.DeleteUserAsync(email);
            if (!success)
            {
                return NotFound("User not found or deletion failed.");
            }

            return Ok("The user has been deleted successfully");
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
