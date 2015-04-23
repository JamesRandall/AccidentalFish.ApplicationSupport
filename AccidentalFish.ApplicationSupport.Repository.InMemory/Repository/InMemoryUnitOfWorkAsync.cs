using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Repository
{
    internal class InMemoryUnitOfWorkAsync : IUnitOfWorkAsync
    {
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly ConcurrentDictionary<Type, object> _uncommittedCollections = new ConcurrentDictionary<Type, object>();

        public InMemoryUnitOfWorkAsync(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        public void Dispose()
        {
            
        }

        public Task<IRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class
        {
            object collectionObject = _uncommittedCollections.GetOrAdd(typeof(T), t => new List<T>());
            List<T> collection = (List<T>)collectionObject;
            return Task.FromResult(_repositoryProvider.GetAsyncRepository<T>(collection));
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(Func<Task> func)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(Func<Task> func, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<T2> ExecuteAsync<T2>(Func<Task<T2>> func)
        {
            throw new NotImplementedException();
        }

        public Task<T2> ExecuteAsync<T2>(Func<Task<T2>> func, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OptimisticRepositoryWinsUpdate(Action update)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OptimisticRepositoryWinsUpdate(Action update, int maxRetries)
        {
            throw new NotImplementedException();
        }

        public bool SuspendExecutionPolicy { get; set; }
    }
}
