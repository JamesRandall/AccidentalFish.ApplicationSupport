using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IUnitOfWorkAsync : IDisposable
    {
        Task<IRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class;
        Task<int> SaveAsync();
        Task<bool> OptimisticRepositoryWinsUpdateAsync(Action update);
        Task<bool> OptimisticRepositoryWinsUpdateAsync(Action update, int maxRetries);
    }
}
