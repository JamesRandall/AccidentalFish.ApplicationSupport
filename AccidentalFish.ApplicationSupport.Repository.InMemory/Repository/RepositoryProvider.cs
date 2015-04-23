using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Repository
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private readonly ConcurrentDictionary<Type, object> _collections = new ConcurrentDictionary<Type, object>();
        
        public IRepositoryAsync<T> GetAsyncRepository<T>(List<T> unitOfWorkChanges) where T : class
        {
            object collectionObject = _collections.GetOrAdd(typeof (T), t => new List<T>());
            List<T> collection = (List<T>) collectionObject;

            return new InMemoryRepositoryAsync<T>(collection, unitOfWorkChanges);
        }
    }
}
