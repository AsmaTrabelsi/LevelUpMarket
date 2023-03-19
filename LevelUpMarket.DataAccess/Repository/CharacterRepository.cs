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
    public class CharacterRepository : Repository<Character>, ICharacterRepository
    {
        private ApplicationDbContext _db;
        public CharacterRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void ICharacterRepository.Update(Character character)
        {            
            _db.Characters.Update(character);
        }
    }
}
