using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    [Table("Medecins")]
    public class Medecin
    {
        [Key]
        public string Id { get; set; } 

        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }

        public string Specialite { get; set; } 
    }
}
