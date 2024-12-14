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

    // Méthode automatique pour l'envoi du nombre de médicaments reçus
    public async Task<int> EnvoyerNombreMedicamentsRecu(int medicamentId)
    {
        var medicament = await context.Medicaments.FindAsync(medicamentId);
        var fournisseur = await context.Fournisseurs.FirstOrDefaultAsync();  // Récupérer le fournisseur

        if (medicament != null && medicament.QttStock <= 10 && fournisseur != null)
        {
            var notification = new Notification
            {
                MedicamentId = medicamentId,
                QuantiteDemandee = 50, // Quantité de réapprovisionnement demandée
                DateNotification = DateTime.Now,
                IsEnvoye = false
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            // Mise à jour du stock du médicament et du fournisseur
            medicament.QttStock += notification.QuantiteDemandee;
            fournisseur.QttStock -= notification.QuantiteDemandee; // Mise à jour du stock du fournisseur

            await context.SaveChangesAsync();

            return notification.QuantiteDemandee;
        }

        return 0; // Retourne 0 si la condition de stock n'est pas remplie
    }

    // Méthode pour envoyer manuellement la quantité au fournisseur
    public async Task Envoyer(int medicamentId, int quantite)
    {
        var medicament = await context.Medicaments.FirstOrDefaultAsync(m => m.Id == medicamentId);
        var fournisseur = await context.Fournisseurs.FirstOrDefaultAsync();  // Récupérer le fournisseur

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

    // Méthode pour envoyer une notification au fournisseur
    public async Task EnvoyerNotification(int medicamentId, int quantite)
    {
        var medicament = await context.Medicaments.FindAsync(medicamentId);
        var fournisseur = await context.Fournisseurs.FirstOrDefaultAsync();  // Récupérer le fournisseur

        if (medicament != null && medicament.QttStock <= 10 && fournisseur != null)
        {
            var notification = new Notification
            {
                MedicamentId = medicamentId,
                QuantiteDemandee = quantite,
                DateNotification = DateTime.Now,
                IsEnvoye = false
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
        }
    }

    // Méthode pour marquer la notification comme envoyée
    public async Task MarquerNotificationCommeEnvoyee(int notificationId)
    {
        var notification = await context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsEnvoye = true;
            await context.SaveChangesAsync();
        }
    }

}
