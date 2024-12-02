using Microsoft.AspNetCore.Identity;
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

            // Configure Medecin relationship with ApplicationUser
            modelBuilder.Entity<Medecin>()
                .HasOne(m => m.ApplicationUser) // Each Medecin has one ApplicationUser
                .WithMany() // An ApplicationUser can have multiple Medecins
                .HasForeignKey(m => m.ApplicationUserId) // Use the correct foreign key property
                .OnDelete(DeleteBehavior.Cascade); // On delete, cascade the action

            // Configure Pharmacien relationship with ApplicationUser
            modelBuilder.Entity<Pharmacien>()
                .HasOne(p => p.ApplicationUser) // Each Pharmacien has one ApplicationUser
                .WithMany() // An ApplicationUser can have multiple Pharmaciens
                .HasForeignKey(p => p.ApplicationUserId) // Use the correct foreign key property
                .OnDelete(DeleteBehavior.Cascade); // On delete, cascade the action

            // Ensure Email and UserName are unique
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email).IsUnique(); // Index for Email uniqueness
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName).IsUnique(); // Index for UserName uniqueness
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Pharmacien",
                NormalizedName = "pharmacien",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            },
            new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Medecin",
                NormalizedName = "medecin",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            });

        }
    }
}
