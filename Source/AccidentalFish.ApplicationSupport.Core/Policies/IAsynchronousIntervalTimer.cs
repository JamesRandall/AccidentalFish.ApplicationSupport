using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Implementations execute tasks based around an interval timer.
    /// </summary>
    public interface IAsynchronousIntervalTimer
    {
        /// <summary>
        /// Execute a task
        /// </summary>
        /// <param name="function">The task to execute, the function should return an awaitable task with a boolean result. The boolean result should be true to cause the timer to repeat, false to exit the timer.</param>
        /// <param name="cancellationToken">Cancellation token used to shutdown both the task and the timer</param>
        /// <param name="shutdownAction">An optional action to execute when the timer completes</param>
        /// <returns></returns>
        Task ExecuteAsync(Func<CancellationToken, Task<bool>> function, CancellationToken cancellationToken, Action shutdownAction=null);
        /// <summary>
        /// Execute a task
        /// </summary>
        /// <param name="function">The task to execute. Should return true to cause the timer to repeat, false to exit the timer.</param>
        /// <param name="shutdownAction">An optional action to execute when the timer completes</param>
        /// <returns></returns>
        Task ExecuteAsync(Func<Task<bool>> function, Action shutdownAction = null);
    }
}