using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository
{
    public class SubtitleRepository : Repository<Subtitle>, ISubtitleRepository
    {
        private ApplicationDbContext _db;
        public SubtitleRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void ISubtitleRepository.Update(Subtitle subtitle)
        {
            _db.Subtitles.Update(subtitle);
        }
    }
}
