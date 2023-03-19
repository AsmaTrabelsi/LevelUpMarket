using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IDeveloperRepository Developer { get; }
        ICharacterRepository Character { get; }
        IGameRepository Game { get; }
        void Save();
    }
}
