namespace ProjetNET.Modeles.Repository
{
    public interface IMedicamentRepository
    {
        Task<List<Medicament>> GetAll();
        Task<Medicament> GetById(int id);
        Task<Medicament> Add(Medicament medicament);
        Task Update(Medicament medicament);
        Task Delete (int id);
            }
}
