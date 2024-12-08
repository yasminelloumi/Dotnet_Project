using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    [Table("Pharmaciens")]
    public class Pharmacien
    {
        
        [Key]
        public string Id { get; set; } 

        [ForeignKey(nameof(Id))] 
        public virtual ApplicationUser User { get; set; } 

        public string LicenseNumber { get; set; } 
    }
}
