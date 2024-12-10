using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    public class Medicament
    {
        [Key]

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
        public float Prix { get; set; }
        public int QttStock { get; set; }
       
       
    }
}
