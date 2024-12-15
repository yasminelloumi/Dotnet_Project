using ProjetNET.DTO;
using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    public class OrdonnanceHistorique
    {
        [Key]
        public int Id { get; set; }

        public string PatientName { get; set; }

        public string MedecinName { get; set; }

        public List<MedicamentHistoriqueDTO> Medicaments { get; set; } = new List<MedicamentHistoriqueDTO>();
        //test
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
