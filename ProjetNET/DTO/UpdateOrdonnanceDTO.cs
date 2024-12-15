namespace ProjetNET.DTO
{
    public class UpdateOrdonnanceDTO
    {
        public int PatientId { get; set; }
        public string MedecinName { get; set; }  // Add this to identify the doctor
        public List<MedicamentQuantityDTO> Medicaments { get; set; }
    }

    
}
