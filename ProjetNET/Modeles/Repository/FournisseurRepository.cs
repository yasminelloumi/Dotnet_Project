using ProjetNET.Modeles.Repository;
using ProjetNET.Modeles;
using Microsoft.EntityFrameworkCore;

public class FournisseurRepository : IFournisseurRepository
{
    private readonly Context context;

    public FournisseurRepository(Context context)
    {
        this.context = context;
    }
    public async Task<List<Fournisseur>> GetAll()
    {
        List<Fournisseur> fournisseurs = await context.Fournisseurs.ToListAsync();
        return fournisseurs;
    }

    // Méthode pour envoyer manuellement la quantité au fournisseur
    public async Task Envoyer(int medicamentId, int quantite)
    {
        var medicament = await context.Medicaments.FirstOrDefaultAsync(m => m.Id == medicamentId);
        var fournisseur = await context.Fournisseurs.FirstOrDefaultAsync();

        if (medicament == null || fournisseur == null)
        {
            throw new Exception($"Le médicament avec l'ID {medicamentId} ou le fournisseur n'a pas été trouvé.");
        }

        // Vérifiez si la quantité à envoyer est supérieure à la quantité en stock
        if (medicament.QttStock < quantite)
        {
            throw new Exception($"La quantité demandée dépasse le stock disponible pour le médicament avec l'ID {medicamentId}.");
        }

        // Mettre à jour le stock du médicament et du fournisseur
        medicament.QttStock += quantite;
        fournisseur.QttStock -= quantite;

        // Enregistrer la notification de l'envoi manuel
        var notification = new Notification
        {
            MedicamentId = medicamentId,
            QuantiteDemandee = quantite,
            DateNotification = DateTime.Now,
            IsEnvoye = true
        };
        context.Notifications.Add(notification);

        await context.SaveChangesAsync();
    }

    public async Task<bool> VerifierDisponibiliteMedicament(DemandeAchat demandeAchat)
    {
        var dbDemandeAchat = await context.DemandesAchats
                                          .FirstOrDefaultAsync(x => x.Id == demandeAchat.Id);
        if (dbDemandeAchat == null)
        {
            throw new Exception("demande d'achat non existante");
        }

        var fournisseur = await context.Fournisseurs
             .FirstOrDefaultAsync(f => f.Id == demandeAchat.MedicamentId);  // Chercher le fournisseur pour le médicament

        if (fournisseur == null)
        {
            throw new Exception("Fournisseur not found");

        }
        if (demandeAchat.Quantite > fournisseur.QttStock)
        {
            return false;
        }
        var medicament = await context.Medicaments
            .FirstOrDefaultAsync(m => m.Id == demandeAchat.MedicamentId);
        if (medicament == null)
        {
            throw new Exception("Medicament not found");
        }
        medicament.QttStock += demandeAchat.Quantite;
        fournisseur.QttStock -= demandeAchat.Quantite;
        context.DemandesAchats.Remove(dbDemandeAchat);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IList<string>> TraitementDemandeAchat(List<DemandeAchat> demandesAchat)
    {
        var retours = new List<string>();
        foreach (var demande in demandesAchat)
        {
            var verif = await VerifierDisponibiliteMedicament(demande);
            var msg = verif ? $"Le médicament {demande.MedicamentId} a été traité avec succès." :
                              $"Le médicament {demande.MedicamentId} n'est pas en stock.";
            retours.Add(msg);
        }
        return retours;
    }



}
