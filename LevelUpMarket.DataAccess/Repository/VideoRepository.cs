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
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        private ApplicationDbContext _db;
        public VideoRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void IVideoRepository.Update(Video video)
        {
            _db.Videos.Update(video);
        }
    }
}
