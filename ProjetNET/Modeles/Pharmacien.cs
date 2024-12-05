using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    [Table("Pharmaciens")]
    public class Pharmacien
    {
        
        [Key]
        public string Id { get; set; } // Primary key (and foreign key)

        [ForeignKey(nameof(Id))] // Establish foreign key relationship with ApplicationUser
        public virtual ApplicationUser User { get; set; } // Virtual navigation property for lazy loading

        public string LicenseNumber { get; set; } // Pharmacien-specific attribute
    }
}
