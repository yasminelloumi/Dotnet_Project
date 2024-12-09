using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjetNET.DTO;
using ProjetNET.Modeles;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly Context _context;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             SignInManager<ApplicationUser> signInManager,
                             Context context,
                             IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
        _configuration = configuration;
    }

    // Register Method
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return BadRequest("A user with this email already exists.");

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

        if (model.Role == "pharmacien")
        {
            if (string.IsNullOrEmpty(model.LicenseNumber))
            {
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

        await _context.SaveChangesAsync();

        var addToRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
        if (!addToRoleResult.Succeeded)
        {
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await _context.SaveChangesAsync();
            return BadRequest(addToRoleResult.Errors);
        }

        return Ok("User registered successfully.");
    }

    // Login Method
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid credentials.");

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetUserProfile()
    {
        try
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine($"Extracted userId: {email}");

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User ID not found in token.");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            object roleSpecificDetails = null;
            if (user.Role == "pharmacien")
            {
                roleSpecificDetails = await _context.Pharmaciens.FindAsync(user.Id);
            }
            else if (user.Role == "medecin")
            {
                roleSpecificDetails = await _context.Medecins.FindAsync(user.Id);
            }

            var profile = new
            {
                user.UserName,
                user.Email,
                user.Role,
                AdditionalDetails = roleSpecificDetails 
            };

            return Ok(profile);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }


    [Authorize]
    [HttpGet("Demo")]
    public IActionResult Demo()
    {
        return Ok("User Authenticated Successfully!");
    }


    // JWT Token Generation
    private string GenerateJwtToken(ApplicationUser user)
    {
        var secretKey = _configuration["JWT:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT:SecretKey configuration is missing.");
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
