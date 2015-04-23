using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Repository
{
    // TODO: This class needs to properly isolate updates.
    internal class InMemoryRepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly IReadOnlyCollection<T> _committedEntities;
        private readonly List<T> _unitOfWorkChanges;

        public InMemoryRepositoryAsync(IReadOnlyCollection<T> committedEntities, List<T> unitOfWorkChanges)
        {
            _committedEntities = committedEntities;
            _unitOfWorkChanges = unitOfWorkChanges;
        }

        public IQueryable<T> All
        {
            get
            {
                //List<T> committedCopy = 
                List<T> combinedList = new List<T>(_committedEntities);
                combinedList.AddRange(_unitOfWorkChanges);
                return combinedList.AsQueryable();
            }
        }
        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return All;
        }

        public Task<T> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(T entity, Func<T, int> idFunc)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
