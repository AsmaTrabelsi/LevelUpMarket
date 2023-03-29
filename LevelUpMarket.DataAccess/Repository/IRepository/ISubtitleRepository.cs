using LevelUpMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository.IRepository
{
    public interface ISubtitleRepository : IRepository<Subtitle>
    {
        void Update(Subtitle subtitle);
    }
}
