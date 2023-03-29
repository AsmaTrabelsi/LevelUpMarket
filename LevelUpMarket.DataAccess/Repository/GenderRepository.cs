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
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        private ApplicationDbContext _db;
        public GenderRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void IGenderRepository.Update(Gender gender)
        {
            _db.Genders.Update(gender);
        }
    }
}
