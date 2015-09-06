using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Allowing the caller to acquire and release leases.
    /// A lease allows the user to perform a lock in a multi-server environment by using a shared locking resource (in Azure blob storage)
    /// </summary>
    /// <typeparam name="T">The type of the lease ID</typeparam>
    public interface ILeaseManager<in T>
    {
        /// <summary>
        /// Create a lease objcet if it doesn't yet exist
        /// </summary>
        /// <param name="key">The key for the lease</param>
        /// <returns>True if the lease was created, false if it already exists</returns>
        Task<bool> CreateLeaseObjectIfNotExistAsync(T key);
        /// <summary>
        /// Acquire a lease with an maximum duration of 30 seconds
        /// </summary>
        /// <param name="key">The key for the lease</param>
        /// <returns>The ID of the lease</returns>
        Task<string> LeaseAsync(T key);

        /// <summary>
        /// Acquire a lease
        /// </summary>
        /// <param name="key">The key for the lease</param>
        /// <param name="leaseTime">The time until the lease expires if it is not released</param>
        /// <returns>The ID of the lease</returns>
        Task<string> LeaseAsync(T key, TimeSpan leaseTime);
        /// <summary>
        /// Release the lease identified by leaseId and key
        /// </summary>
        /// <param name="key">The key for the lease</param>
        /// <param name="leaseId">The ID of the acquired lease</param>
        /// <returns>An awaitable task</returns>
        Task ReleaseAsync(T key, string leaseId);
        /// <summary>
        /// Renews the lease
        /// </summary>
        /// <param name="key">The key for the lease</param>
        /// <param name="leaseId">The ID of the acquired lease</param>
        Task RenewAsync(T key, string leaseId);
    }
}
