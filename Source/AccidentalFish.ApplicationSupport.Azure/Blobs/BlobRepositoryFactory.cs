using System;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class BlobRepositoryFactory : IBlobRepositoryFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IAzureAssemblyLogger _logger;

        public BlobRepositoryFactory(IConfiguration configuration,
            IAzureAssemblyLogger logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;
            _logger = logger;
        }

        public IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string containerName)
        {
            if (String.IsNullOrWhiteSpace(containerName)) throw new ArgumentNullException(nameof(containerName));

            return new AsynchronousBlockBlobRepository(_configuration.StorageAccountConnectionString, containerName, _logger);
        }

        public IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string storageAccountConnectionString,
            string blobContainerName)
        {
            if (String.IsNullOrWhiteSpace(storageAccountConnectionString)) throw new ArgumentNullException(nameof(storageAccountConnectionString));
            if (String.IsNullOrWhiteSpace(blobContainerName)) throw new ArgumentNullException(nameof(blobContainerName));

            return new AsynchronousBlockBlobRepository(storageAccountConnectionString, blobContainerName, _logger);
        }
    }
}
