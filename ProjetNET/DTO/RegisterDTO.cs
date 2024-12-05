using System.ComponentModel.DataAnnotations;

namespace ProjetNET.DTO
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; } // Optional: "admin", "medecin", "pharmacien"
    }
}
