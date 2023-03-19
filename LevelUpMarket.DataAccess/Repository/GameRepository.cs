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
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private ApplicationDbContext _db;
        public GameRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void IGameRepository.Update(Game game)
        {
            _db.Games.Update(game);
        }

    
    }
}
