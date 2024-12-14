using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public int QttSortie { get; set; }

        [JsonIgnore]
        public List<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();

        // Méthode pour ajuster le stock du médicament
        public void AjouterStock(int quantiteAjoutee)
        {
            if (quantiteAjoutee > 0)
            {
                QttStock += quantiteAjoutee;
            }
        }
    }
}
