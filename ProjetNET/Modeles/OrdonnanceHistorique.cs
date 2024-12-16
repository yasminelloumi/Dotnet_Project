using System.ComponentModel.DataAnnotations;

public class MedicamentHistorique
{
    [Key]
    public int Id { get; set; } // Clé primaire
    public string Nom { get; set; }
    public int Quantite { get; set; }

    public int OrdonnanceHistoriqueId { get; set; } // Clé étrangère
    public OrdonnanceHistorique OrdonnanceHistorique { get; set; } // Navigation inverse
}

public class OrdonnanceHistorique
{
    [Key]
    public int Id { get; set; } // Clé primaire
    public string PatientName { get; set; }
    public string MedecinName { get; set; }

    public List<MedicamentHistorique> Medicaments { get; set; } // Navigation
    public DateTime CreationDate { get; set; }
}
