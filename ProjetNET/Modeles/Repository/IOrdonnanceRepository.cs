namespace ProjetNET.Modeles.Repository
{
    public interface IOrdonnanceRepository
    {
        Task<Ordonnance> Add(Ordonnance ordonnance);
       
        Task<List<Ordonnance>> GetAll();
        Task<Ordonnance> GetById(int id);
        Task Update(Ordonnance ordonnance);
    }
}
