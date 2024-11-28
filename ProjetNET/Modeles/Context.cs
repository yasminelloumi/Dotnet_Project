using Microsoft.EntityFrameworkCore;

namespace ProjetNET.Modeles
{
    public class Context: DbContext
    {
        public Context (DbContextOptions options):
        
            base(options){ }
            public DbSet<Medicament> Medicaments { get; set; }

    }

    
}
