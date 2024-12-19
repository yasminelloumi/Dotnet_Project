using ProjetNET.DTO;
using ProjetNET.Modeles.Repository;
using ProjetNET.Modeles;
using Microsoft.EntityFrameworkCore;

public class MedicamentOrdonnanceRepository : IMedicamentOrdonnanceRepository
{
    private readonly Context context;

    public MedicamentOrdonnanceRepository(Context context)
    {
        this.context = context;
    }

    public async Task<bool> VerifierDisponibiliteMedicament(MedicamentOrdonnance medicamentOrdonnance)
    {
        var dbMedicamentOrdonnance = await context.MedicamentOrdonnances
                                                   .FirstOrDefaultAsync(x => x.Id == medicamentOrdonnance.Id);
        if (dbMedicamentOrdonnance == null)
        {
            throw new Exception("Demande de médicament non existante.");
        }

        var medicament = await context.Medicaments
                                      .FirstOrDefaultAsync(m => m.Id == medicamentOrdonnance.IDMedicament);
        if (medicament == null)
        {
            throw new Exception("Médicament introuvable.");
        }

        if (medicamentOrdonnance.Quantite > medicament.QttStock)
        {
            return false;
        }

        medicament.QttStock -= medicamentOrdonnance.Quantite;
        context.MedicamentOrdonnances.Remove(dbMedicamentOrdonnance);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IList<string>> TraitementDemandeOrdonnance(List<MedicamentOrdonnance> medicamentOrdonnances)
    {
        var retours = new List<string>();
        foreach (var medicamentOrdonnance in medicamentOrdonnances)
        {
            var verif = await VerifierDisponibiliteMedicament(medicamentOrdonnance);
            var msg = verif
                ? $"Le médicament {medicamentOrdonnance.IDMedicament} a été traité avec succès."
                : $"Le médicament {medicamentOrdonnance.IDMedicament} n'est pas en stock.";
            retours.Add(msg);
        }
        return retours;
    }

    // New method to handle the entire order processing
    public async Task ProcessOrdonnanceAsync(CreateOrdonnanceDTO createOrdonnanceDTO)
    {
        var patient = await context.Patients.FindAsync(createOrdonnanceDTO.PatientId);
        if (patient == null)
        {
            throw new ArgumentException("Patient introuvable.");
        }

        var medecin = await context.Medecins
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.User.UserName == createOrdonnanceDTO.MedecinName);

        if (medecin == null)
        {
            throw new ArgumentException("Médecin introuvable.");
        }

        // Create Ordonnance entity and add the medications to MedicamentOrdonnance table
        var ordonnance = new Ordonnance
        {
            PatientId = patient.ID,
            MedecinId = medecin.Id,
            MedicamentOrdonnances = new List<MedicamentOrdonnance>()
        };

        foreach (var medicamentDTO in createOrdonnanceDTO.Medicaments)
        {
            var medicament = await context.Medicaments.FindAsync(medicamentDTO.MedicamentId);
            if (medicament == null)
            {
                throw new ArgumentException($"Médicament avec l'ID {medicamentDTO.MedicamentId} introuvable.");
            }

            // Check if the requested quantity is available
            if (medicamentDTO.Quantite > medicament.QttStock)
            {
                throw new ArgumentException($"La quantité de {medicament.Name} est insuffisante.");
            }

            // Create MedicamentOrdonnance record
            var medicamentOrdonnance = new MedicamentOrdonnance
            {
                IDMedicament = medicament.Id,
                Quantite = medicamentDTO.Quantite,
                Medicament = medicament
            };

            // Update stock
            medicament.QttStock -= medicamentDTO.Quantite;

            // Add to Ordonnance
            ordonnance.MedicamentOrdonnances.Add(medicamentOrdonnance);
        }

        // Save the Ordonnance and MedicamentOrdonnances
        await context.Ordonnances.AddAsync(ordonnance);
        await context.SaveChangesAsync();

        // Now, delete the records from MedicamentOrdonnance table after processing
        foreach (var medicamentOrdonnance in ordonnance.MedicamentOrdonnances)
        {
            context.MedicamentOrdonnances.Remove(medicamentOrdonnance);
        }

        await context.SaveChangesAsync();
    }
}
