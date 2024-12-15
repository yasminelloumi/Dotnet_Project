using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace ProjetNET.Modeles
{
    public class Patient
    {
        [Key]
        public int ID { get; set; }
        public string NamePatient { get; set; }
        public DateTime DateOfBirth { get; set; }

        [JsonIgnore]
        public List<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();

        // Propriété calculée pour afficher l'historique des médicaments
        //public string Historique
        //{
        //    get
        //    {
        //        // Concatène les noms des médicaments associés à ce patient
        //        return string.Join(", ", Ordonnances.SelectMany(o => o.Medicaments).Select(m => m.Name).Distinct());
        //    }
        //}
    }


}
