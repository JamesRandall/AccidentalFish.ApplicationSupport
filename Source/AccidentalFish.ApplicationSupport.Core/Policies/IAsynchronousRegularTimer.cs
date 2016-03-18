using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Implementations execute tasks based around a regular timer. Note that tasks running under this timer will be permitted to run
    /// no longer than the interval the timer was created with - should they overrun then a cancellation will be triggered.
    /// </summary>
    public interface IAsynchronousRegularTimer
    {
        /// <summary>
        /// Execute a task
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="cancellationToken">Cancellation token used to shutdown both the task and the timer</param>
        /// <param name="shutdownAction">An optional action to execute when the timer completes</param>
        /// <returns>An awaitable task that completes when the timer is cancelled.</returns>
        Task ExecuteAsync(Action<CancellationToken> action, CancellationToken cancellationToken, Action shutdownAction = null);
    }
}