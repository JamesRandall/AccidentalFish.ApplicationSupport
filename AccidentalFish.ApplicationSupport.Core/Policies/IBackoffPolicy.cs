using System;
using System.Threading;
using AccidentalFish.ApplicationSupport.Core.Threading;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// As opposed to a retry policy a back off policy doesn't fail at a maximum number of retries but continues to try.
    /// Generally used with a queue.
    /// </summary>
    public interface IBackoffPolicy
    {
        /// <summary>
        /// Execute the back off policy running the specified function
        /// </summary>
        /// <param name="function">
        /// The function to run - should return true if it was able to do work, false if not.
        /// False will trigger a move to the next stage of the backoff policy.
        /// If the function throws an exception the backoff policy will exit and the exception will bubble
        /// up to the caller of Execute.
        /// </param>
        /// <param name="waitHandle">
        /// If a wait handle is supplied then it is integrated into the back off policy such that the policies backoff
        /// sleep times are passed through to the wait handle.
        /// </param>
        void Execute(Func<bool> function, IWaitHandle waitHandle);
        /// <summary>
        /// Execute the back off policy running the specified function terminating when the cancellation token is signalled
        /// </summary>
        /// <param name="function">
        /// The function to run - should return true if it was able to do work, false if not.
        /// False will trigger a move to the next stage of the backoff policy.
        /// If the function throws an exception the backoff policy will exit and the exception will bubble
        /// up to the caller of Execute.
        /// </param>
        /// <param name="cancellationToken">
        /// The policy will run until the cancellation token is set
        /// </param>
        void Execute(Func<bool> function, CancellationToken cancellationToken);
    }
}
