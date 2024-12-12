using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjetNET.Modeles
{
    public class Ordonnance
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public string MedecinId { get; set; }
        public Medecin Medecin { get; set; }

        public ICollection<Medicament> Medicaments { get; set; } = new List<Medicament>();
    }
}
