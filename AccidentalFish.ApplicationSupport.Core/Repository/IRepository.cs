using System;
using System.Linq;
using System.Linq.Expressions;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        T Find(int id);
        void InsertOrUpdate(T account, Func<T, int> idFunc);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
