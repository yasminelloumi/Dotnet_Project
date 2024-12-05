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