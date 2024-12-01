using System.ComponentModel.DataAnnotations;

namespace ProjetNET.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
