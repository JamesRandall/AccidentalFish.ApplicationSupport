using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    public interface IAsynchronousBackoffPolicy
    {
        Task Execute(Func<Task<bool>> function, CancellationToken token);
        Task Execute(Func<Task<bool>> function, Action shutdownAction, CancellationToken cancellationToken);
    }
}