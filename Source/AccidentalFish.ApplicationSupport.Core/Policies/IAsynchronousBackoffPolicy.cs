using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Asynchronous backoff policy
    /// </summary>
    public interface IAsynchronousBackoffPolicy
    {
        /// <summary>
        /// Execute an action inside the backoff policy
        /// </summary>
        /// <param name="function">Function to execute - should return true if it the function had work to perform, false if it did not and the back off policy
        /// needs to begin</param>
        /// <param name="token">A cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task ExecuteAsync(Func<Task<bool>> function, CancellationToken token);

        /// <summary>
        /// Execute an action inside the backoff policy
        /// </summary>
        /// <param name="function">Function to execute - should return true if it the function had work to perform, false if it did not and the back off policy
        /// needs to begin</param>
        /// <param name="shutdownAction">Action that is called as the policy shuts down</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task ExecuteAsync(Func<Task<bool>> function, Action shutdownAction, CancellationToken cancellationToken);
    }
}
