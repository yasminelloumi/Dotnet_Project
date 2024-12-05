using Microsoft.AspNetCore.Identity;

namespace ProjetNET.Modeles
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
