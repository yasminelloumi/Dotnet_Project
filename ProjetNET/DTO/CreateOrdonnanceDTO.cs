namespace ProjetNET.DTO
{
    public class CreateOrdonnanceDTO
    {
        public int PatientId { get; set; }
        public string MedecinName { get; set; }
        public List<int> MedicamentIds { get; set; } // Liste des IDs des médicaments à inclure
    }

    public class OrdonnanceResponseDTO
    {
        public int Id { get; set; }
        public string MedecinName { get; set; }
        public string PatientName { get; set; }
        public List<string> Medicaments { get; set; } // Noms des médicaments
    }
}
