using System;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class BlobRepositoryFactory : IBlobRepositoryFactory
    {
        private readonly IConfiguration _configuration;

        public BlobRepositoryFactory(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        public IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string containerName)
        {
            if (String.IsNullOrWhiteSpace(containerName)) throw new ArgumentNullException("containerName");

            return new AsynchronousBlockBlobRepository(_configuration.StorageAccountConnectionString, containerName);
        }

        public IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string storageAccountConnectionString,
            string blobContainerName)
        {
            if (String.IsNullOrWhiteSpace(storageAccountConnectionString)) throw new ArgumentNullException("storageAccountConnectionString");
            if (String.IsNullOrWhiteSpace(blobContainerName)) throw new ArgumentNullException("blobContainerName");

            return new AsynchronousBlockBlobRepository(storageAccountConnectionString, blobContainerName);
        }
    }
}
