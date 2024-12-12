using ProjetNET.DTO;

namespace ProjetNET.Modeles.Repository
{
    public interface IOrdonnanceRepository
    {
        Task<OrdonnanceResponseDTO> CreateOrdonnance(CreateOrdonnanceDTO dto);
        Task<OrdonnanceResponseDTO> GetOrdonnanceAsDTO(int id);
        Task<IEnumerable<OrdonnanceResponseDTO>> GetAllOrdonnances(); // Nouvelle méthode
        Task<bool> DeleteOrdonnance(int id); // Nouvelle méthode
        Task<OrdonnanceResponseDTO> UpdateOrdonnance(int id, UpdateOrdonnanceDTO dto); // Nouvelle méthode
        Task<List<Ordonnance>> SearchOrdonnances(string medecinId, int? patientId);
    }
}
