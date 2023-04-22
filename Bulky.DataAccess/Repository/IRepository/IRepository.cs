using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T will be category or any other generic model on which we want to perform certain operations
        IEnumerable<T> GetAll(string? includeProperteies = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperteies = null, bool tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
