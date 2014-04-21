using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository.Implementaton
{
    internal sealed class EntityFrameworkRepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly DbContext _context;

        public EntityFrameworkRepositoryAsync(DbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }

        public IQueryable<T> All
        {
            get { return _context.Set<T>(); }
        }

        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(All, (current, includeProperty) => current.Include(includeProperty));
        }        

        public Task<T> FindAsync(int id)
        {
            return _context.Set<T>().FindAsync(id);
        }

        public void InsertOrUpdate(T entity, Func<T, int> idFunc)
        {
            if (idFunc(entity) == default(int))
            {
                // New entity
                _context.Set<T>().Add(entity);
            }
            else
            {
                // Existing entity
                _context.Entry(entity).State = EntityState.Modified;
            }
            
        }

        public void InsertAsync(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await FindAsync(id);
            _context.Set<T>().Remove(entity);
        }
    }
}
