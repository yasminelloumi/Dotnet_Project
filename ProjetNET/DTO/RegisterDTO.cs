using Azure.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjetNET.DTO
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } 

        public string? LicenseNumber { get; set; } 
        public string? Specialite { get; set; } 
    }
}
