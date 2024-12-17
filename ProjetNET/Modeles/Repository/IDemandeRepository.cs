namespace ProjetNET.Modeles.Repository
{
    public interface IDemandeRepository
    {
        Task<IEnumerable<DemandeAchat>> GetAllDemandesAsync();
        Task AjouterDemandeAsync(DemandeAchat demande);
        Task ConfirmerAchatAsync(int demandeId);
    }
}
