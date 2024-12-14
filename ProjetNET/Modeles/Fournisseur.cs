using System.ComponentModel.DataAnnotations;

namespace ProjetNET.Modeles
{
    public class Fournisseur
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Prix { get; set; }
        public int QttStock { get; set; }
        public int QttSortie { get; set; } // Quantité sortie de stock
        public List<Notification> Notifications { get; set; } = new List<Notification>(); // Liens avec les notifications envoyées

    }
}
