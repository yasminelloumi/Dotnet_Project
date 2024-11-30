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
            base.OnModelCreating(modelBuilder);

            // Configuration de la hiérarchie avec la colonne 'role' comme discriminant
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("role") // Utiliser 'role' comme colonne discriminante
                .HasValue<User>("user")
                .HasValue<Medecin>("medecin")
                .HasValue<Pharmacien>("pharmacien");

            // Indices uniques sur email et username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.username).IsUnique();
        }




    }

}

    

