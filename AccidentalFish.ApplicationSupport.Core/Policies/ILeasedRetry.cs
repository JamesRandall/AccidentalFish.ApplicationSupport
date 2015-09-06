using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Allows an operation to be run inside a lease block ensuring that the operation will never happen in parallel even
    /// in a multi-thread / multi-server environment.
    /// </summary>
    public interface ILeasedRetry
    {
        /// <summary>
        /// Attempt to run an operation inside a lease block with upto 10 retries and a window of 30 seconds before the lease expires.
        /// The lease object (blob) must already exist before this method is called.
        /// </summary>
        /// <typeparam name="T">The type of the key to use for the lease</typeparam>
        /// <param name="leaseManager">A lease manager implementation</param>
        /// <param name="key">The key to use with the lease manager</param>
        /// <param name="func">The action to operate</param>
        /// <returns>True if the operation was executed, false if not</returns>
        Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, Func<Task> func);

        /// <summary>
        /// Attempt to run an operation inside a lease block with upto 10 retries and a window of 30 seconds before the lease expires.
        /// </summary>
        /// <typeparam name="T">The type of the key to use for the lease</typeparam>
        /// <param name="leaseManager">A lease manager implementation</param>
        /// <param name="key">The key to use with the lease manager</param>
        /// <param name="createLazyLeaseObject">Set to false if the lease object should not be attempted to be created, true if it should be created if it doesn't exist</param>
        /// <param name="func">The action to operate</param>
        /// <returns>True if the operation was executed, false if not</returns>
        Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, bool createLazyLeaseObject, Func<Task> func);

        /// <summary>
        /// Attempt to run an operation inside a lease block with upto 10 retries.
        /// The lease object (blob) must already exist before this method is called.
        /// </summary>
        /// <typeparam name="T">The type of the key to use for the lease</typeparam>
        /// <param name="leaseManager">A lease manager implementation</param>
        /// <param name="key">The key to use with the lease manager</param>
        /// <param name="leaseDuration">The length of time to hold the lease before it expires</param>
        /// <param name="func">The action to operate</param>
        /// <returns>True if the operation was executed, false if not</returns>
        Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, Func<Task> func);

        /// <summary>
        /// Attempt to run an operation inside a lease block.
        /// The lease object (blob) must already exist before this method is called.
        /// </summary>
        /// <typeparam name="T">The type of the key to use for the lease</typeparam>
        /// <param name="leaseManager">A lease manager implementation</param>
        /// <param name="key">The key to use with the lease manager</param>
        /// <param name="leaseDuration">The length of time to hold the lease before it expires</param>
        /// <param name="maxRetries">The maximum number of times to attempt to acquire the lease</param>
        /// <param name="func">The action to operate</param>
        /// <returns>True if the operation was executed, false if not</returns>
        Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, int maxRetries, Func<Task> func);

        /// <summary>
        /// Attempt to run an operation inside a lease block with upto 10 retries.
        /// </summary>
        /// <typeparam name="T">The type of the key to use for the lease</typeparam>
        /// <param name="leaseManager">A lease manager implementation</param>
        /// <param name="key">The key to use with the lease manager</param>
        /// <param name="leaseDuration">The length of time to hold the lease before it expires</param>
        /// <param name="createLazyLeaseObject">Set to false if the lease object should not be attempted to be created, true if it should be created if it doesn't exist</param>
        /// <param name="func">The action to operate</param>
        /// <returns>True if the operation was executed, false if not</returns>
        Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, bool createLazyLeaseObject, Func<Task> func);

        /// <summary>
        /// Attempt to run an operation inside a lease block.
        /// The lease object (blob) must already exist before this method is called.
        /// </summary>
        /// <typeparam name="T">The type of the key to use for the lease</typeparam>
        /// <param name="leaseManager">A lease manager implementation</param>
        /// <param name="key">The key to use with the lease manager</param>
        /// <param name="leaseDuration">The length of time to hold the lease before it expires</param>
        /// <param name="maxRetries">The maximum number of times to attempt to acquire the lease</param>
        /// <param name="createLazyLeaseObject">Set to false if the lease object should not be attempted to be created, true if it should be created if it doesn't exist</param>
        /// <param name="func">The action to operate</param>
        /// <returns>True if the operation was executed, false if not</returns>
        Task<bool> RetryAsync<T>(ILeaseManager<T> leaseManager, T key, TimeSpan leaseDuration, int maxRetries, bool createLazyLeaseObject, Func<Task> func);
    }
}
