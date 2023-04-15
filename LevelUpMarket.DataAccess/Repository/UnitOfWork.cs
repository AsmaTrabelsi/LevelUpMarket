using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepository(_db);
            Developer =new DeveloperRepository(_db);
            Character = new CharacterRepository(_db);
            Game = new GameRepository(_db);
            Plateforme = new PlateformeRepository(_db);
            Subtitle = new SubtitleRepository(_db);
            Gender = new GenderRepository(_db);
            Video = new VideoRepository(_db);
            VoiceLanguage = new VoiceLanguageRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IDeveloperRepository Developer { get; private set; }
        public ICharacterRepository Character { get; private set; }
        public IGameRepository Game { get; private set; }
        public IPlateformeRepository Plateforme { get; private set; }

        public ISubtitleRepository Subtitle { get; private set; }
        public IVoiceLanguageRepository  VoiceLanguage { get; private set; }
        public IGenderRepository Gender { get; private set; }
        public IVideoRepository Video { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }


        void IUnitOfWork.Save()
        {
            _db.SaveChanges();
        }

        void IUnitOfWork.deleteFroGamePlateforme(int gameId)
        {
            _db.Database.ExecuteSqlRaw("delete from GamePlateforme where gamesId = " + gameId);
        }

        void IUnitOfWork.deleteFroGameGender(int gameId)
        {
            _db.Database.ExecuteSqlRaw("delete from GameGenders where gamesId = " + gameId);
        }

        void IUnitOfWork.deleteFroGameSubtitle(int gameId)
        {
            _db.Database.ExecuteSqlRaw("delete from GameSubtitle where gamesId = " + gameId);

           
        }

        void IUnitOfWork.deleteFroGameVoiceLanguage(int gameId)
        {
            _db.Database.ExecuteSqlRaw("delete from GameVoiceLanguages where gamesId = " + gameId);
        }
    }
}
