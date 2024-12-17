using Microsoft.EntityFrameworkCore;
using ProjetNET.Modeles;

namespace ProjetNET.Modeles.Repository
{
    public class DemandeRepository : IDemandeRepository
    {
        private readonly Context _context;

        public DemandeRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DemandeAchat>> GetAllDemandesAsync()
        {
            return await _context.DemandesAchats.ToListAsync();
        }

        public async Task AjouterDemandeAsync(DemandeAchat demande)
        {
            _context.DemandesAchats.Add(demande);
            await _context.SaveChangesAsync();
        }

        public async Task ConfirmerAchatAsync(int demandeId)
        {
            var demande = await _context.DemandesAchats.FindAsync(demandeId);
            if (demande != null)
            {
                demande.Statut = "Validée";
                await _context.SaveChangesAsync();
            }
        }
    }
}
