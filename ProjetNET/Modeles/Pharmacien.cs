namespace ProjetNET.Modeles
{
    public class Pharmacien
    {
        public int Id { get; set; }
        public string LicenseNumber { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
