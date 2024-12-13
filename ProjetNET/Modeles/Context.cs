using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

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


            // Relation Ordonnance -> Medicaments (many-to-many)
            modelBuilder.Entity<Ordonnance>()
                .HasMany(o => o.Medicaments)
                .WithMany(m => m.Ordonnances);

            // Configure OrdonnanceHistorique
            modelBuilder.Entity<OrdonnanceHistorique>()
                .Property(h => h.MedicamentNames)
                .HasConversion(
                    v => string.Join(",", v), // Convert List to CSV
                    v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList() // Convert CSV back to List
                )
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2), // Compare lists for equality
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Generate hash code for the list
                    c => c.ToList())); // Create a deep copy
        }
    }
}
