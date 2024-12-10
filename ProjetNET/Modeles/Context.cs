using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjetNET.Modeles
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Pharmacien> Pharmaciens { get; set; }
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<Ordonnance> Ordonnances { get; set; }

        


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

            modelBuilder.Entity<Ordonnance>()
               .HasMany(o => o.Medicaments)
               .WithMany(m => m.Ordonnances);
        }
    }
}
