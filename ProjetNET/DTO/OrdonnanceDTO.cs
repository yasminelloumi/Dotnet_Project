namespace ProjetNET.DTO
{
    public class OrdonnanceDTO
    {
        public int Id { get; set; }
        public string MedecinName { get; set; }
        public string PatientName { get; set; }
        public List<string> Medicaments { get; set; } // Contient uniquement les noms des médicaments
    }
}
