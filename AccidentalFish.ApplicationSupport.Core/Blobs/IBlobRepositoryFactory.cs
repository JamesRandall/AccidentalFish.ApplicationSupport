namespace AccidentalFish.ApplicationSupport.Core.Blobs
{
    public interface IBlobRepositoryFactory
    {
        IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string containerName);
        IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string storageAccountConnectionString, string blobContainerName);
    }
}
