using LevelUpMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository.IRepository
{
    public interface IVideoRepository : IRepository<Video>
    {
        void Update(Video subtitle);
    }
}
