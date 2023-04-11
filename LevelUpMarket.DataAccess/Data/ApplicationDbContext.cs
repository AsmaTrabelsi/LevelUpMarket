using Microsoft.EntityFrameworkCore;
using LevelUpMarket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LevelUpMarket.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Plateforme> Plateformes { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<VoiceLanguage> VoiceLanguages { get; set; }
        public DbSet<Subtitle> Subtitles { get; set; }

        public DbSet<Game> Games { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
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
            // relation one to many - game and characters - 
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Characters)
                .WithOne(i => i.Game)
                .HasForeignKey(i => i.GameId);
            // relation many to many - game and subtitle - 
            modelBuilder.Entity<Game>()
               .HasMany(g => g.Subtitles)
               .WithMany(p => p.Games)
                .UsingEntity(j => j.ToTable("GameSubtitle"));
            // relation many to many - game and voice language - 
            modelBuilder.Entity<Game>()
                .HasMany(g => g.VoiceLanguages)
                .WithMany(p => p.Games)
                 .UsingEntity(j => j.ToTable("GameVoiceLanguages"));
            // relation many to many - game and Gender - 
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Genders)
                .WithMany(p => p.Games)
                 .UsingEntity(j => j.ToTable("GameGenders"));


            base.OnModelCreating(modelBuilder);

        }

    }
}
