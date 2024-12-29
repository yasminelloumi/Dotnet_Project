namespace ProjetNET.Modeles.Repository
{
    public interface IFournisseurRepository
    {
        Task<List<Fournisseur>> GetAll();
        Task<bool> VerifierDisponibiliteMedicament(DemandeAchat demandeAchat);
        Task<IList<string>> TraitementDemandeAchat(List<DemandeAchat> demandesAchat);


    }
}
