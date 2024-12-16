using ProjetNET.DTO;

namespace ProjetNET.Modeles.Repository
{
    public interface IOrdonnanceRepository
    {
        Task<OrdonnanceResponseDTO> CreateOrdonnanceAsync(CreateOrdonnanceDTO dto); // Creating an Ordonnance
        Task<OrdonnanceResponseDTO> GetOrdonnanceAsDTO(int id); // Get a specific Ordonnance as DTO
        Task<IEnumerable<OrdonnanceResponseDTO>> GetAllOrdonnances(); // Get all Ordonnances as DTOs
        Task<bool> DeleteOrdonnance(int id); // Delete a specific Ordonnance
        Task<Ordonnance> UpdateOrdonnanceAsync(int id, UpdateOrdonnanceDTO dto); // Update an existing Ordonnance
        Task<IEnumerable<OrdonnanceResponseDTO>> SearchOrdonnances(string medecinId, int? patientId); // Search Ordonnances by Medecin and Patient
        Task<List<OrdonnanceHistorique>> GetHistoriqueAsync(); // Get the history of Ordonnances
    }
}
