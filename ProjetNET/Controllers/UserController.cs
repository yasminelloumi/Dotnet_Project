
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Context _context;




        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, Context context)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;


        }
     

        [HttpGet("{userId}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpGet("List Of Users")]
        public async Task<ActionResult<ApplicationUser>> GetAllUsers()
        {
            var user = await _userRepository.GetAllUsersAsync();
            return Ok(user);
        }


        [HttpPost("Create User")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest("A user with this email already exists.");

            // Check if the role exists
            if (!await _roleManager.RoleExistsAsync(model.Role))
                return BadRequest($"The role '{model.Role}' does not exist.");

            // Create new user
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Role = model.Role
            };

            var createUserResult = await _userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
                return BadRequest(createUserResult.Errors);

            // Role-specific handling
            if (model.Role == "pharmacien")
            {
                if (string.IsNullOrEmpty(model.LicenseNumber))
                {
                    // Delete user if required data is missing
                    await _userManager.DeleteAsync(user);
                    return BadRequest("LicenseNumber is required for pharmacien.");
                }

                var pharmacien = new Pharmacien
                {
                    Id = user.Id,
                    LicenseNumber = model.LicenseNumber
                };
                _context.Pharmaciens.Add(pharmacien);
            }
            else if (model.Role == "medecin")
            {
                if (string.IsNullOrEmpty(model.Specialite))
                {
                    // Delete user if required data is missing
                    await _userManager.DeleteAsync(user);
                    return BadRequest("Specialite is required for medecin.");
                }

                var medecin = new Medecin
                {
                    Id = user.Id,
                    Specialite = model.Specialite
                };
                _context.Medecins.Add(medecin);
            }

            // Save additional role-specific data to the database
            await _context.SaveChangesAsync();

            // Assign the user to the specified role
            var addToRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
            if (!addToRoleResult.Succeeded)
            {
                // Clean up user and related data if role assignment fails
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                await _context.SaveChangesAsync();
                return BadRequest(addToRoleResult.Errors);
            }

            return Ok("User added successfully.");
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<ApplicationUser>> UpdateUser(string userId, [FromBody] ApplicationUser updatedUser)
        {
            try
            {
                if (updatedUser == null)
                {
                    return BadRequest(new { Message = "User data is required" });
                }

                if (string.IsNullOrEmpty(updatedUser.UserName) || string.IsNullOrEmpty(updatedUser.Email))
                {
                    return BadRequest(new { Message = "Username and email are required" });
                }

                var user = await _userRepository.UpdateUserAsync(userId, updatedUser.UserName, updatedUser.Email);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
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

        /*
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
        */
    }
}
