using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IUnitOfWorkAsync : IDisposable
    {
        Task<IRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class;
        Task<int> SaveAsync();
        Task ExecuteAsync(Func<Task> func);
        Task ExecuteAsync(Func<Task> func, CancellationToken token);
        Task<T2> ExecuteAsync<T2>(Func<Task<T2>> func);
        Task<T2> ExecuteAsync<T2>(Func<Task<T2>> func, CancellationToken token);
        bool SuspendExecutionPolicy { get; set; }
    }
}
