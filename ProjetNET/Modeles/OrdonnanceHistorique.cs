using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    public class OrdonnanceHistorique
    {
        [Key]
        public int Id { get; set; }

        public string PatientName { get; set; }

        public string MedecinName { get; set; }

        public List<string> MedicamentNames { get; set; } = new List<string>();
        //test
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
