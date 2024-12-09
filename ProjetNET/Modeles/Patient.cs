using System.ComponentModel.DataAnnotations;
namespace ProjetNET.Modeles
{
    public class Patient
    {
        [Key]
        public int ID { get; set; }
        public string NamePatient { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MedicalHistory { get; set; }
    }

}
