namespace ProjetNET.Modeles
{
    public class Admin
    {
        // Identifiant unique pour l'admin
        public int Id { get; set; }

        // Clé étrangère vers ApplicationUser
        public string ApplicationUserId { get; set; }

        // Relation avec la classe ApplicationUser
        public ApplicationUser ApplicationUser { get; set; }
    }
}
