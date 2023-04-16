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
        public DbSet<Orderheader> Orderheaders { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed data

            // for Plateforme
            modelBuilder.Entity<Plateforme>().HasData(
                new Plateforme { Id=1, Name="PC"},
                new Plateforme { Id = 2, Name = "PS2" },
                new Plateforme { Id = 3, Name = "PS3" },
                new Plateforme { Id = 4, Name = "PS4" },
                new Plateforme { Id = 5, Name = "PS5" },
                new Plateforme { Id = 6, Name = "XBox" }
                );
            // for Gender
            modelBuilder.Entity<Gender>().HasData(
               new Gender { Id = 1, Name = "Action" },
               new Gender { Id = 2, Name = "Adventure" },
               new Gender { Id = 3, Name = "Strategy" },
               new Gender { Id = 4, Name = "Sport" },
               new Gender { Id = 5, Name = "Story rich" },
               new Gender { Id = 6, Name = "Zombies" },
               new Gender { Id = 7, Name = "Viloent" },
               new Gender { Id = 8, Name = "Single player" },
               new Gender { Id = 9, Name = "Emotional" },
               new Gender { Id = 10, Name = "Horror" },
               new Gender { Id = 11, Name = "Drama" },
               new Gender { Id = 12, Name = "Multiplayer" },
               new Gender { Id = 13, Name = "Remake" },
               new Gender { Id = 14, Name = "Survival" },
               new Gender { Id = 15, Name = "First Person Shooter" },
               new Gender { Id = 16, Name = "Puzzle" },
               new Gender { Id = 17, Name = "RPG" },
               new Gender { Id = 18, Name = "Third-person shooter" }
               );

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
