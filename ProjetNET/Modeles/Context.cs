using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using ProjetNET.DTO;

namespace ProjetNET.Modeles
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Pharmacien> Pharmaciens { get; set; }
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<Ordonnance> Ordonnances { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<OrdonnanceHistorique> OrdonnanceHistoriques { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MedicamentOrdonnance> MedicamentOrdonnances { get; set; }
        // Enable lazy loading proxies
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Medecin and Pharmacien relationships
            modelBuilder.Entity<Medecin>()
                .HasOne(m => m.User)
                .WithOne()
                .HasForeignKey<Medecin>(m => m.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pharmacien>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Pharmacien>(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);

            //Historique
            // Relation Patient -> Ordonnance
            modelBuilder.Entity<Ordonnance>()
                .HasOne(o => o.Patient)
                .WithMany(p => p.Ordonnances);

            
            // Optionnel : Configurations supplémentaires pour les champs (si nécessaire)
            modelBuilder.Entity<Medicament>()
                .Property(m => m.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Ordonnance>()
                .Property(o => o.PatientId)
                .IsRequired();

            modelBuilder.Entity<MedicamentOrdonnance>()
            .HasOne(mo => mo.Ordonnance)
            .WithMany(o => o.MedicamentOrdonnances)
            .HasForeignKey(mo => mo.IDOrdonnance)
            .OnDelete(DeleteBehavior.Cascade);  // Vérifiez le comportement ici
        
        modelBuilder.Entity<MedicamentOrdonnance>()
            .HasOne(mo => mo.Medicament)
            .WithMany(m => m.MedicamentOrdonnances)
            .HasForeignKey(mo => mo.IDMedicament)
            .OnDelete(DeleteBehavior.Cascade);  // Vérifiez le comportement ici



            // Configuration pour OrdonnanceHistorique

            modelBuilder.Ignore<MedicamentHistoriqueDTO>();

        }
    }
}
