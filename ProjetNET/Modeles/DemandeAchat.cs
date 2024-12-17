using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    public class DemandeAchat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MedicamentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0.")]
        public int Quantite { get; set; }

        public string Statut { get; set; } = "En attente"; // Statut : En attente, Validée, Rejetée

        public DateTime DateDemande { get; set; } = DateTime.Now;
    }
}
