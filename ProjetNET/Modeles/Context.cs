using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjetNET.Modeles
{
    public class Context: IdentityDbContext<ApplicationUser>
    {
        public Context (DbContextOptions options):
        
            base(options){ }
            public DbSet<Medicament> Medicaments { get; set; }
            public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurer la hiérarchie des rôles avec TPH
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Role")
                .HasValue<User>("user")
                .HasValue<Medecin>(Role.Medecin)
                .HasValue<Pharmacien>(Role.Pharmacien);

            // Configurer les colonnes spécifiques pour Medecin et Pharmacien
            modelBuilder.Entity<Medecin>()
                .Property(m => m.specialite)
                .HasMaxLength(100);

            modelBuilder.Entity<Pharmacien>()
                .Property(p => p.licenseNumber)
                .HasMaxLength(50);

            // Configurer l'index unique sur le champ Email et Username pour tous les utilisateurs
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.username).IsUnique();
        }

        
    }

}

    

