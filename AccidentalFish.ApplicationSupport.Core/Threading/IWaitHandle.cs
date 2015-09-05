namespace AccidentalFish.ApplicationSupport.Core.Threading
{
    /// <summary>
    /// Wrapper for a threading wait handle. Supplied implementation wraps a manual reset event.
    /// </summary>
    public interface IWaitHandle
    {
        /// <summary>
        /// Wait for the handle to be set.
        /// </summary>
        /// <param name="timeout">The maximum time to wait</param>
        /// <returns>True if the handle was set, false if the handle was not set and the timeout reached</returns>
        bool Wait(int timeout);
        /// <summary>
        /// Reset the wait handle
        /// </summary>
        void Reset();
        /// <summary>
        /// Is the handle set
        /// </summary>
        bool IsSet { get; }
    }
}
