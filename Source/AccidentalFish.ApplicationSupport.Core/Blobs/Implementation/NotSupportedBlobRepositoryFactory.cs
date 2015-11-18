using System;

namespace AccidentalFish.ApplicationSupport.Core.Blobs.Implementation
{
    internal class NotSupportedBlobRepositoryFactory : IBlobRepositoryFactory
    {
        public IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string containerName)
        {
            throw new NotImplementedException();
        }

        public IAsynchronousBlockBlobRepository CreateAsynchronousBlockBlobRepository(string storageAccountConnectionString,
            string blobContainerName)
        {
            throw new NotImplementedException();
        }
    }
}