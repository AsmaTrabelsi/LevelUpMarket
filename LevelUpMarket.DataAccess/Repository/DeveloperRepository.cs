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
    public class DeveloperRepository : Repository<Developer>, IDeveloperRepository
    {
        private ApplicationDbContext _db;
        public DeveloperRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void IDeveloperRepository.Update(Developer developer)
        {
            var developerFromDb = _db.Developers.FirstOrDefault(d=>d.Id == developer.Id);
            if (developerFromDb != null)
            {
                developerFromDb.Name = developer.Name;  
               
            }
            _db.Developers.Update(developer);
        }
    }
}
