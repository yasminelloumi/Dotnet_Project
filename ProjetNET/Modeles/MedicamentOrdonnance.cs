using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Modeles
{
    public class MedicamentOrdonnance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        
        public int IDOrdonnance { get; set; }
        public virtual Ordonnance Ordonnance { get; set; }

        [Required]
        
        public int IDMedicament { get; set; }
        public virtual Medicament Medicament { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0.")]
        public int Quantite { get; set; }

    }
}
