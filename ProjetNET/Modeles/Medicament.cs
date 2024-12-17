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
        public List<MedicamentOrdonnance> MedicamentOrdonnances { get; set; } = new List<MedicamentOrdonnance>();



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
