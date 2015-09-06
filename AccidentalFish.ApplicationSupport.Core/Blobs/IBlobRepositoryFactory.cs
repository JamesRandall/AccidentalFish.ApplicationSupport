namespace AccidentalFish.ApplicationSupport.Core.Blobs
{
    /// <summary>
    /// Factory for creating a blob repository
    /// </summary>
    public interface IBlobRepositoryFactory
    {
        /// <summary>
        /// Create a blob repository implementation looking at the specified container
        /// </summary>
        /// <param name="containerName">The container for the blobs</param>
        /// <returns>A blob repository implementation</returns>
        IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string containerName);

        /// <summary>
        /// Create a blob repository implementation looking at the specified container in the specified storage account
        /// </summary>
        /// <param name="storageAccountConnectionString">Connection string to use for accessing the blob container</param>
        /// <param name="blobContainerName">The container for the blobs</param>
        /// <returns>A blob repository implementation</returns>
        IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string storageAccountConnectionString, string blobContainerName);
    }
}
