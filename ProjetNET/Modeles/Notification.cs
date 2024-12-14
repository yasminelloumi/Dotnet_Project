using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int MedicamentId { get; set; }
        public int QuantiteDemandee { get; set; }
        public DateTime DateNotification { get; set; }
        public bool IsEnvoye { get; set; } // Flag pour savoir si la notification a été envoyée
    }
}

