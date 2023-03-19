using Microsoft.EntityFrameworkCore;
using LevelUpMarket.Models;

namespace LevelUpMarket.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}
