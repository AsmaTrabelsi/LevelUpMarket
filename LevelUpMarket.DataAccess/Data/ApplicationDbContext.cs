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
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Plateforme> Plateformes { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // relation many to many - game and plateforme-
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Plateformes)
                .WithMany(p => p.Games)
                 .UsingEntity(j => j.ToTable("GamePlateforme"));

            // relation one to many - game and image - 
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Images)
                .WithOne(i => i.Game)
                .HasForeignKey(i => i.GameId);
            // relation one to many - game and video - 
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Videos)
                .WithOne(i => i.Game)
                .HasForeignKey(i => i.GameId);

            base.OnModelCreating(modelBuilder);

        }

    }
}
