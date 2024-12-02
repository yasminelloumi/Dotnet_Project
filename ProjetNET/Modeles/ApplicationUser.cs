using Microsoft.AspNetCore.Identity;

namespace ProjetNET.Modeles
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } // "admin", "medecin", "pharmacien"

        // Optional profiles for Medecin and Pharmacien
        public Medecin MedecinProfile { get; set; }
        public Pharmacien PharmacienProfile { get; set; }

        // Propriétés de navigation ajoutées pour les relations One-to-Many
        public ICollection<Medecin> Medecins { get; set; }
        public ICollection<Pharmacien> Pharmaciens { get; set; }
    }
}
