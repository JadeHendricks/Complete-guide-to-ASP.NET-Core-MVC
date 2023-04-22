using System.Linq.Expressions;
using Bulky.DataAccess.Repository.IRepository;
using BulkyWeb.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            //this is like writing this - _db.Categories == dbSet, whatever we pass tot he generic will correlate to the table
            //so now when we want to add, remove, edit etc we will call that on the dbSet (dbSet.Add()) as dbSet === <T> what is passed + _db.<T>
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            //ie - _db.categories.Where(u=>u.Id === id).FirstOrDefault(); - _db.Set<T>() aka _dbSet.Categories.Where().FirstOrDefault
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            } else
            {
                query = dbSet.AsNoTracking();
            }

           
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                //split the includeProperties by comma and remove empty entires
                foreach (var includeProp in includeProperties.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            //List<Category> objCategoryList = _db.Categories.ToList(); - aka _dbSet.Categories.ToList()
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            if (!string.IsNullOrEmpty(includeProperties))
            {
                //split the includeProperties by comma and remove empty entires
                foreach (var includeProp in includeProperties.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            //_db.Set<T>() - aka _dbSet.Categories.Remove(entity)
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            //the RemoveRange expects a IEnumerable and it will remove all everything that exists in the IEnumerable
            //_db.Set<T>() - aka _dbSet.Categories.RemoveRange(entity)
            dbSet.RemoveRange(entity);
        }
    }
}
