﻿using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository.IRepository;
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
            VoiceLanguage = new VoiceLanguageRepository(_db);
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



        void IUnitOfWork.Save()
        {
            _db.SaveChanges();
        }
    }
}
