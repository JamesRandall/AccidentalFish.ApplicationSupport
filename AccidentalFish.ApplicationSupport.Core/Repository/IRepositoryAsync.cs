using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IRepositoryAsync<T> where T : class
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindAsync(int id);
        void InsertOrUpdate(T entity, Func<T, int> idFunc);
        void InsertAsync(T entity);
        void UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
