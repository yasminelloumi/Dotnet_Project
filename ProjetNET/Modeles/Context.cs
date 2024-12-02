using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjetNET.Modeles
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<Pharmacien> Pharmaciens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            
            modelBuilder.Entity<Medecin>()
                .HasOne(m => m.ApplicationUser) // Medecin has one ApplicationUser
                .WithOne(u => u.MedecinProfile) // ApplicationUser has one MedecinProfile
                .HasForeignKey<Medecin>(m => m.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete when ApplicationUser is deleted

            // Configure one-to-one relationship between ApplicationUser and Pharmacien
            modelBuilder.Entity<Pharmacien>()
                .HasOne(p => p.ApplicationUser) // Pharmacien has one ApplicationUser
                .WithOne(u => u.PharmacienProfile) // ApplicationUser has one PharmacienProfile
                .HasForeignKey<Pharmacien>(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete when ApplicationUser is deleted

            // Configure unique indexes for Email and UserName
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName).IsUnique();
        }


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
