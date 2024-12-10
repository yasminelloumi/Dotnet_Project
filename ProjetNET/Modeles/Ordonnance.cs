using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjetNET.Modeles
{
    public class Ordonnance
    {
        [Key]
        public int IDOrdonnance { get; set; } 

        public DateTime Date { get; set; }

    
        public int IDPatient { get; set; }
        [ForeignKey(nameof(IDPatient))]
        [JsonIgnore]
        public Patient? Patient { get; set; }

      
        public string IDMedecin { get; set; }
        [ForeignKey(nameof(IDMedecin))]
      
        public Medecin? Medecin { get; set; } 

        // List of associated Medicaments (many-to-many relationship)
        public List<Medicament> Medicaments { get; set; } = new List<Medicament>();
    }
}
