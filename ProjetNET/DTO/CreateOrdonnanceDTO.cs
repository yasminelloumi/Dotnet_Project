using ProjetNET.Modeles;

namespace ProjetNET.DTO
{
    public class CreateOrdonnanceDTO
    {
        public int PatientId { get; set; }
        public string MedecinName { get; set; }
        public List<MedicamentQuantityDTO> Medicaments { get; set; } = new List<MedicamentQuantityDTO>();
    }
    public class MedicamentQuantityDTO
    {
        public int MedicamentId { get; set; }
        public int Quantite { get; set; }
    }


    public class OrdonnanceResponseDTO
    {
        public int Id { get; set; }
        public string MedecinName { get; set; }
        public string PatientName { get; set; }
        public List<MedicamentQuantityDTO> Medicaments { get; set; } = new List<MedicamentQuantityDTO>();


    }

    /////
    

}