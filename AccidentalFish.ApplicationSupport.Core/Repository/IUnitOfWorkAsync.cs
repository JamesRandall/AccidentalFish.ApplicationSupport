using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IUnitOfWorkAsync : IDisposable
    {
        [Obsolete("There is no reason for this method to utilise a task and therefore this will be removed in the next major version. Use GetRepository<T> instead.")]
        Task<IRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class;
        IRepositoryAsync<T> GetRepository<T>() where T : class;
        Task<int> SaveAsync();
        Task<bool> OptimisticRepositoryWinsUpdateAsync(Action update);
        Task<bool> OptimisticRepositoryWinsUpdateAsync(Action update, int maxRetries);
    }
}
