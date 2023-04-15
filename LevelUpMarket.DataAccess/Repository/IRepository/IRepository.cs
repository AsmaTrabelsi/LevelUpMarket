using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //When changes are made to an entity object that is retrieved from the database,
        //Entity Framework tracks these changes, and when you call SaveChanges method,
        //it generates the appropriate SQL statements to update the database with the changes.
        // without calling any methods to update these changes
        T GetFirstOrDefault(Expression<Func<T,bool>>filter, string? includeProperties = null,bool tracked=true);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null,string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        bool Any(Expression<Func<T, bool>> filter);
    }
}
