/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjetNET.DTO;
using ProjetNET.Modeles;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ProjetNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByNameAsync(registerDTO.Username);
            if (user != null)
            {
                return BadRequest("Utilisateur existant");
            }
            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
            };
            var result = await userManager.CreateAsync(applicationUser, registerDTO.Password);
            if (result.Succeeded)
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByNameAsync(login.username);
            if (user == null)
            {
                return BadRequest("Wrong Credentials");
            }
            if (await userManager.CheckPasswordAsync(user, login.password))
            {
                var claims = new List<Claim>();
                //claims.Add(new Claim("name", "value")); 
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti,
Guid.NewGuid().ToString()));
                //signingCredentials 
                var key = new
SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims: claims,
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: sc
                    );
                var _token = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    username = login.username,
                };
                return Ok(_token);
            }
            return BadRequest("Wrong Credentials");
        }
    }
}
*/
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
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly Context _context;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             Context context,
                             IConfiguration configuration)
    {
        _userManager = userManager;
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

        // Check if user already exists
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
            return BadRequest("User with this email already exists.");

        // Create new user
        user = new ApplicationUser { UserName = model.Email, Email = model.Email, Role = model.Role };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Role-specific handling
        if (model.Role == "pharmacien")
        {
            if (string.IsNullOrEmpty(model.LicenseNumber))
                return BadRequest("LicenseNumber is required for pharmacien.");

            var pharmacien = new Pharmacien { Id = user.Id, LicenseNumber = model.LicenseNumber };
            _context.Pharmaciens.Add(pharmacien);
        }
        else if (model.Role == "medecin")
        {
            if (string.IsNullOrEmpty(model.Specialite))
                return BadRequest("Specialite is required for medecin.");

            var medecin = new Medecin { Id = user.Id, Specialite = model.Specialite };
            _context.Medecins.Add(medecin);
        }

        // Save to the database
        await _context.SaveChangesAsync();

        // Assign the role to the user
        await _userManager.AddToRoleAsync(user, model.Role);

        return Ok("User registered successfully");
    }

    // Login Method
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid credentials");

        // Generate JWT token and return it
        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
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
            issuer: _configuration["JWT:issuer"],
            audience: _configuration["JWT:audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

