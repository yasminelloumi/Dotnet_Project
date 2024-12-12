namespace ProjetNET.DTO
{
    public class UpdateOrdonnanceDTO
    {
        public int PatientId { get; set; }
        public List<int> MedicamentIds { get; set; }
    }
}
