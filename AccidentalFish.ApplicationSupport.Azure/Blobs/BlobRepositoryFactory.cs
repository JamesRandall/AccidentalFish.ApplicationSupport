using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using CuttingEdge.Conditions;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class BlobRepositoryFactory : IBlobRepositoryFactory
    {
        private readonly IConfiguration _configuration;

        public BlobRepositoryFactory(IConfiguration configuration)
        {
            Condition.Requires(configuration).IsNotNull();
            _configuration = configuration;
        }

        public IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string containerName)
        {
            Condition.Requires(containerName).IsNotNullOrWhiteSpace();
            return new AsynchronousBlockBlobRepository(_configuration.StorageAccountConnectionString, containerName);
        }

        public IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string storageAccountConnectionString,
            string blobContainerName)
        {
            Condition.Requires(blobContainerName).IsNotNull();
            Condition.Requires(storageAccountConnectionString).IsNotNull();
            return new AsynchronousBlockBlobRepository(storageAccountConnectionString, blobContainerName);
        }
    }
}
