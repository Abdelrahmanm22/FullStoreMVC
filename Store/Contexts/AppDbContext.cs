using Microsoft.EntityFrameworkCore;

namespace Store.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options):base(options) 
        {
            
        }
    }
}
