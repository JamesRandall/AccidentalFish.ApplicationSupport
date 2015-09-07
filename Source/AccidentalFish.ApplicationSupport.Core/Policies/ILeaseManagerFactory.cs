namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Factory for the lease manager
    /// </summary>
    public interface ILeaseManagerFactory
    {
        /// <summary>
        /// Create a lease manager
        /// </summary>
        /// <typeparam name="T">The type of the lease key</typeparam>
        /// <param name="storageAccountConnectionString">The connection string for the lease resource (in Azure this is a storage account)</param>
        /// <param name="leaseBlockName">The container name for leases (in Azure this is a blob container)</param>
        /// <returns>A lease manager</returns>
        ILeaseManager<T> CreateLeaseManager<T>(string storageAccountConnectionString, string leaseBlockName);
    }
}
