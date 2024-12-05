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
        public string Role { get; set; } // Role: medecin/pharmacien

        // Specific attributes for roles
        public string? LicenseNumber { get; set; } // For pharmacien
        public string? Specialite { get; set; } // For medecin
    }
}
