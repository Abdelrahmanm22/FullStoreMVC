using Microsoft.EntityFrameworkCore;
using Store.Models;

namespace Store.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options):base(options) 
        {
            
        }
        public DbSet<Product> Products { get; set; }

    }
}
