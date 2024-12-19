
using Microsoft.EntityFrameworkCore;
using ProjetNET.DTO;

namespace ProjetNET.Modeles.Repository
{
    public class MedicamentRepository : IMedicamentRepository
    {
        private readonly Context context;
        public MedicamentRepository(Context context)
        {
            this.context = context;
            
        }

        public  async Task<Medicament> Add(Medicament medicament)
        {
            var result = await context.Medicaments.AddAsync(medicament);
            await context.SaveChangesAsync();
            return result.Entity
                ;
        }

        public async Task Delete(int id)
        {
            Medicament med = await context.Medicaments.FindAsync(id);
            context.Medicaments.Remove(med);
            await context.SaveChangesAsync();
        }

        public async Task<List<Medicament>> GetAll()
        {
            List<Medicament> medicaments = await context.Medicaments.ToListAsync();
            return medicaments;
        }

        public async Task<Medicament> GetById(int id)
        {
            Medicament med = await context.Medicaments.FindAsync(id);
            return med;
        }

        public async Task Update(Medicament medicament)
        {
            context.Medicaments.Update(medicament);
            await context.SaveChangesAsync();
        }

        // Méthode de recherche par nom ou ID
        public async Task<List<Medicament>> SearchMedicament(string searchTerm)
        {
            var query = context.Medicaments.AsQueryable();

            // Si le terme de recherche est un nombre, on suppose que c'est un ID
            if (int.TryParse(searchTerm, out int id))
            {
                // Recherche par ID
                query = query.Where(m => m.Id == id);
            }
            else
            {
                // Recherche par nom
                query = query.Where(m => m.Name.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }
        public async Task<string> GetMedicamentsEnSeuilAsync()
        {
            var medicamentsEnSeuil = await context.Medicaments
                                                     .Where(m => m.QttStock <= 10)
                                                     .ToListAsync();

            // Vérifier si la liste est vide
            if (!medicamentsEnSeuil.Any())
            {
                return "Aucun médicament en seuil critique.";
            }

            // Construire la chaîne avec les médicaments en seuil critique
            var medicamentsMessage = string.Join(", ", medicamentsEnSeuil.Select(m => $"ID: {m.Id}, Nom: {m.Name}"));

            // Retourner le message final
            return $"Il y a des médicaments qui sont à 10 ou moins en quantité: {medicamentsMessage}.";
        }


        public async Task AjouterStockMedicamentAsync(int medicamentId, int quantite)
        {
            var medicament = await context.Medicaments.FindAsync(medicamentId);
            if (medicament != null)
            {
                medicament.QttStock += quantite;
                await context.SaveChangesAsync();
            }
        }

        // Récupère les médicaments en seuil critique avec un DTO pour le formulaire
        public async Task<List<MedicamentDemandeDto>> GetMedicamentsEnSeuilPourDemandeAsync()
        {
            var results = await context.Medicaments
                .Where(m => m.QttStock <= 10)
                .Select(m => new MedicamentDemandeDto
                {
                    MedicamentId = m.Id,
                    MedicamentName = m.Name,
                    QuantiteDemandee = 0
                })
                .ToListAsync();

            // Log the result count
            Console.WriteLine($"Retrieved {results.Count} medicaments below threshold.");
            return results;
        }

        // Ajoute les demandes de médicaments dans une table des demandes
        public async Task<bool> AjouterDemandeMedicamentAsync(List<MedicamentDemandeDto> demandes)
        {
            if (demandes == null || !demandes.Any())
                return false;

            var demandesAchats = demandes.Select(d => new DemandeAchat
            {
                MedicamentId = d.MedicamentId,
                Quantite = d.QuantiteDemandee,
                Statut = "En attente", // Statut initial
                DateDemande = DateTime.Now
            }).ToList();

            await context.DemandesAchats.AddRangeAsync(demandesAchats);
            await context.SaveChangesAsync();
            return true;
        }


    }
}

