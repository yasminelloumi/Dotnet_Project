namespace ProjetNET.Modeles.Repository
{
    public interface IFournisseurRepository
    {
        Task<List<Fournisseur>> GetAll();
        Task<int> EnvoyerNombreMedicamentsRecu(int medicamentId);
        Task Envoyer(int medicamentId, int quantite);
        Task EnvoyerNotification(int medicamentId, int quantite);
        Task<bool> VerifierDisponibiliteMedicament(DemandeAchat demandeAchat);
        Task<IList<string>> TraitementDemandeAchat(List<DemandeAchat> demandesAchat);


    }
}
