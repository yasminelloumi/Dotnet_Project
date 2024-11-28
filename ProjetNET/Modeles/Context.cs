using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjetNET.Modeles
{
    public class Context: IdentityDbContext<ApplicationUser>
    {
        public Context (DbContextOptions options):
        
            base(options){ }
            public DbSet<Medicament> Medicaments { get; set; }

    }

    
}
