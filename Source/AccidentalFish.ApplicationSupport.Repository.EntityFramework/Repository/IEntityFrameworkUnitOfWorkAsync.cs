using System;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    public interface IEntityFrameworkUnitOfWorkAsync : IUnitOfWorkAsync
    {
        Task ExecuteAsync(Func<Task> func);
        Task ExecuteAsync(Func<Task> func, CancellationToken token);
        Task<T2> ExecuteAsync<T2>(Func<Task<T2>> func);
        Task<T2> ExecuteAsync<T2>(Func<Task<T2>> func, CancellationToken token);
        bool SuspendExecutionPolicy { get; set; }
    }
}
