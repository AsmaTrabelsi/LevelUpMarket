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
    public class PlateformeRepository : Repository<Plateforme>, IPlateformeRepository
    {
        private ApplicationDbContext _db;
        public PlateformeRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void IPlateformeRepository.Update(Plateforme plateforme)
        {
            _db.Plateformes.Update(plateforme);
        }
    }
}
