using ProjetNET.DTO;

namespace ProjetNET.Modeles.Repository
{
    public interface IMedicamentOrdonnanceRepository
    {
        Task<bool> VerifierDisponibiliteMedicament(MedicamentOrdonnance medicamentOrdonnance);
        Task<IList<string>> TraitementDemandeOrdonnance(List<MedicamentOrdonnance> medicamentOrdonnances);
        Task ProcessOrdonnanceAsync(CreateOrdonnanceDTO createOrdonnanceDTO);
    }

}
