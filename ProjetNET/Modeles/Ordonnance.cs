using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetNET.Modeles
{
    public class Ordonnance
    {
        public int Id { get; set; }

        // Ensure Patient relationship is configured correctly
        [Required] // Mark PatientId as required
        public int PatientId { get; set; }

        // Add 'virtual' to enable lazy loading
        public virtual Patient Patient { get; set; }

        // Ensure Medecin relationship is configured correctly
        [Required] // Mark MedecinId as required
        public string MedecinId { get; set; }

        // Add 'virtual' to enable lazy loading
        public virtual Medecin Medecin { get; set; }

        // Use virtual for lazy loading and initialize the collection
        //public virtual ICollection<Medicament> Medicaments { get; set; } = new List<Medicament>();

        // Relation avec MedicamentOrdonnance (table intermédiaire)
        public List<MedicamentOrdonnance> MedicamentOrdonnances { get; set; } = new List<MedicamentOrdonnance>();


    }
}
