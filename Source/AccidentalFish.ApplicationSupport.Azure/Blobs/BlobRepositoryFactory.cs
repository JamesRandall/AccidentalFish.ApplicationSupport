using System;
using AccidentalFish.ApplicationSupport.Azure.Extensions;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class BlobRepositoryFactory : IBlobRepositoryFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public BlobRepositoryFactory(IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;
            _logger = loggerFactory.GetAssemblyLogger();
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
