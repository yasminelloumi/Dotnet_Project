using ProjetNET.DTO;

namespace ProjetNET.Modeles.Repository
{
    public interface IMedicamentRepository
    {
        Task<List<Medicament>> GetAll();
        Task<Medicament> GetById(int id);
        Task<Medicament> Add(Medicament medicament);
        Task Update(Medicament medicament);
        Task Delete (int id);
        Task<List<Medicament>> SearchMedicament(string searchTerm);
        Task<string> GetMedicamentsEnSeuilAsync();
        Task AjouterStockMedicamentAsync(int medicamentId, int quantite);
        Task<List<MedicamentDemandeDto>> GetMedicamentsEnSeuilPourDemandeAsync();
        Task<bool> AjouterDemandeMedicamentAsync(List<MedicamentDemandeDto> demandes);


    }
}
