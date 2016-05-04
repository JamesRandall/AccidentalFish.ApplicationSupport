using System;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Core.Queues.Implementation
{
    internal class LargeMessageQueueFactory : ILargeMessageQueueFactory
    {
        private readonly IApplicationResourceSettingProvider _applicationResourceSettingProvider;
        private readonly IQueueFactory _queueFactory;
        private readonly IBlobRepositoryFactory _blobRepositoryFactory;
        private readonly IQueueSerializer _serializer;
        private readonly ICoreAssemblyLogger _logger;

        public LargeMessageQueueFactory(
            IApplicationResourceSettingProvider applicationResourceSettingProvider,
            IQueueFactory queueFactory,
            IBlobRepositoryFactory blobRepositoryFactory,
            IQueueSerializer serializer,
            ICoreAssemblyLogger logger)
        {
            _applicationResourceSettingProvider = applicationResourceSettingProvider;
            _queueFactory = queueFactory;
            _blobRepositoryFactory = blobRepositoryFactory;
            _serializer = serializer;
            _logger = logger;
        }

        public ILargeMessageQueue<T> Create<T>(IComponentIdentity componentIdentity) where T : class
        {
            return Create<T>(componentIdentity, null);
        }

        public ILargeMessageQueue<T> Create<T>(IComponentIdentity componentIdentity, ILargeMessageQueueErrorHandler errorHandler) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultQueueName = _applicationResourceSettingProvider.DefaultQueueName(componentIdentity);
            string blobContainerName = _applicationResourceSettingProvider.DefaultBlobContainerName(componentIdentity);
            IAsynchronousQueue<LargeMessageReference> referenceQueue = _queueFactory.CreateAsynchronousQueue<LargeMessageReference>(storageAccountConnectionString, defaultQueueName);
            IAsynchronousBlockBlobRepository blobRepository = _blobRepositoryFactory.CreateAsynchronousBlockBlobRepository(storageAccountConnectionString, blobContainerName);

            return Create<T>(referenceQueue, blobRepository, errorHandler);
        }

        public ILargeMessageQueue<T> Create<T>(string connectionString, string queueName, string blobContainerName) where T : class
        {
            return Create<T>(connectionString, queueName, blobContainerName, null);
        }

        public ILargeMessageQueue<T> Create<T>(string connectionString, string queueName, string blobContainerName, ILargeMessageQueueErrorHandler errorHandler) where T : class
        {
            IAsynchronousQueue<LargeMessageReference> referenceQueue = _queueFactory.CreateAsynchronousQueue<LargeMessageReference>(connectionString, queueName);
            IAsynchronousBlockBlobRepository blobRepository = _blobRepositoryFactory.CreateAsynchronousBlockBlobRepository(connectionString, blobContainerName);

            ILargeMessageQueue<T> queue = new LargeMessageQueue<T>(_serializer, referenceQueue, blobRepository, _logger, errorHandler);
            return queue;
        }

        public ILargeMessageQueue<T> Create<T>(IAsynchronousQueue<LargeMessageReference> referenceQueue, IAsynchronousBlockBlobRepository blobRepository) where T : class
        {
            return Create<T>(referenceQueue, blobRepository, null);
        }

        public ILargeMessageQueue<T> Create<T>(
            IAsynchronousQueue<LargeMessageReference> referenceQueue,
            IAsynchronousBlockBlobRepository blobRepository,
            ILargeMessageQueueErrorHandler errorHandler) where T : class
        {
            return new LargeMessageQueue<T>(_serializer, referenceQueue, blobRepository, _logger, errorHandler);
        }
    }
}
