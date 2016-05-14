using System;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Extensions;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Core.Components.Implementation
{
    internal class ApplicationResourceFactory : IApplicationResourceFactory
    {
        private readonly IApplicationResourceSettingProvider _applicationResourceSettingProvider;
        private readonly IApplicationResourceSettingNameProvider _nameProvider;
        private readonly IQueueFactory _queueFactory;
        private readonly IBlobRepositoryFactory _blobRepositoryFactory;
        private readonly IConfiguration _configuration;
        private readonly ILeaseManagerFactory _leaseManagerFactory;
        private readonly IUnitOfWorkFactoryProvider _unitOfWorkFactoryProvider;
        private readonly ICoreAssemblyLogger _logger;

        public ApplicationResourceFactory(
            IApplicationResourceSettingProvider applicationResourceSettingProvider,
            IApplicationResourceSettingNameProvider nameProvider,
            IQueueFactory queueFactory,
            IBlobRepositoryFactory blobRepositoryFactory,
            IConfiguration configuration,
            ILeaseManagerFactory leaseManagerFactory,
            IUnitOfWorkFactoryProvider unitOfWorkFactoryProvider,
            ICoreAssemblyLogger logger)
        {
            _applicationResourceSettingProvider = applicationResourceSettingProvider;
            _nameProvider = nameProvider;
            _queueFactory = queueFactory;
            _blobRepositoryFactory = blobRepositoryFactory;
            _configuration = configuration;
            _leaseManagerFactory = leaseManagerFactory;
            _unitOfWorkFactoryProvider = unitOfWorkFactoryProvider;
            _logger = logger;
        }

        public IUnitOfWorkFactory GetUnitOfWorkFactory(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            _logger?.Verbose("ApplicationResourceFactory - GetUnitOfWorkFactory - {0}", componentIdentity);

            string sqlConnectionString = _applicationResourceSettingProvider.SqlConnectionString(componentIdentity);
            string contextType = _applicationResourceSettingProvider.SqlContextType(componentIdentity);
            return _unitOfWorkFactoryProvider.Create(contextType, sqlConnectionString);
        }

        public ILeaseManager<T> GetLeaseManager<T>(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            _logger?.Verbose("ApplicationResourceFactory - GetLeaseManager - {0}", componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultLeaseBlockName = _applicationResourceSettingProvider.DefaultLeaseBlockName(componentIdentity);
            return _leaseManagerFactory.CreateLeaseManager<T>(storageAccountConnectionString, defaultLeaseBlockName);
        }

        public ILeaseManager<T> GetLeaseManager<T>(string leaseBlockName, IComponentIdentity componentIdentity)
        {
            if (String.IsNullOrWhiteSpace(leaseBlockName)) throw new ArgumentNullException(nameof(leaseBlockName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetLeaseManager - {0},{1}", leaseBlockName, componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _leaseManagerFactory.CreateLeaseManager<T>(storageAccountConnectionString, leaseBlockName);
        }

        public IAsynchronousQueue<T> GetAsyncQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncQueue - {0}", componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultQueueName = _applicationResourceSettingProvider.DefaultQueueName(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, defaultQueueName);
        }

        public IAsynchronousQueue<T> GetAsyncQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncQueue - {0},{1}", queuename, componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, queuename);
        }

        public IAsynchronousQueue<T> GetAsyncQueue<T>(IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncQueue - {0}", componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultQueueName = _applicationResourceSettingProvider.DefaultQueueName(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, defaultQueueName, queueSerializer);
        }

        public IAsynchronousQueue<T> GetAsyncQueue<T>(string queuename, IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncQueue - {0},{1}", queuename, componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, queuename, queueSerializer);
        }

        public IQueue<T> GetQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetQueue - {0}", componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultQueueName = _applicationResourceSettingProvider.DefaultQueueName(componentIdentity);
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, defaultQueueName);
        }

        public IQueue<T> GetQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetQueue - {0},{1}", queuename, componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, queuename);
        }

        public IQueue<T> GetQueue<T>(IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetQueue - {0}", componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultQueueName = _applicationResourceSettingProvider.DefaultQueueName(componentIdentity);
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, defaultQueueName, queueSerializer);
        }

        public IQueue<T> GetQueue<T>(string queuename, IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetQueue - {0},{1}", queuename, componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, queuename, queueSerializer);
        }

        public IAsynchronousTopic<T> GetAsyncTopic<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncTopic - {0}", componentIdentity);

            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            string defaultTopicName = _applicationResourceSettingProvider.DefaultTopicName(componentIdentity);
            return _queueFactory.CreateAsynchronousTopic<T>(serviceBusConnectionString, defaultTopicName);
        }

        public IAsynchronousTopic<T> GetAsyncTopic<T>(string topicName, IComponentIdentity componentIdentity) where T : class
        {
            if (String.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncTopic - {0},{1}", topicName, componentIdentity);

            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            return _queueFactory.CreateAsynchronousTopic<T>(serviceBusConnectionString, topicName);
        }

        public IAsynchronousSubscription<T> GetAsyncSubscription<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncSubscription - {0}", componentIdentity);

            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            string defaultTopicName = _applicationResourceSettingProvider.DefaultTopicName(componentIdentity);
            string defaultSubscriptionName = _applicationResourceSettingProvider.DefaultSubscriptionName(componentIdentity);
            return _queueFactory.CreateAsynchronousSubscription<T>(serviceBusConnectionString, defaultTopicName, defaultSubscriptionName);
        }

        public IAsynchronousSubscription<T> GetAsyncSubscription<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class
        {
            if (String.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException(nameof(subscriptionName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncSubscription - {0},{1}", subscriptionName, componentIdentity);

            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            string defaultTopicName = _applicationResourceSettingProvider.DefaultTopicName(componentIdentity);
            return _queueFactory.CreateAsynchronousSubscription<T>(serviceBusConnectionString, defaultTopicName, subscriptionName);
        }

        public IAsynchronousSubscription<T> GetAsyncSubscription<T>(string subscriptionName, string topicName, IComponentIdentity componentIdentity) where T : class
        {
            if (String.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException(nameof(subscriptionName));
            if (String.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncSubscription - {0},{1},{2}", subscriptionName, topicName, componentIdentity);

            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            return _queueFactory.CreateAsynchronousSubscription<T>(serviceBusConnectionString, topicName, subscriptionName);
        }

        public IAsynchronousBlockBlobRepository GetAsyncBlockBlobRepository(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncBlockBlobRepository - {0}", componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string blobContainerName = _applicationResourceSettingProvider.DefaultBlobContainerName(componentIdentity);
            return _blobRepositoryFactory.CreateAsynchronousBlockBlobRepository(storageAccountConnectionString, blobContainerName);
        }

        public IAsynchronousBlockBlobRepository GetAsyncBlockBlobRepository(string containerName, IComponentIdentity componentIdentity)
        {
            if (String.IsNullOrWhiteSpace(containerName)) throw new ArgumentNullException(nameof(containerName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncBlockBlobRepository - {0},{1}", containerName, componentIdentity);

            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _blobRepositoryFactory.CreateAsynchronousBlockBlobRepository(storageAccountConnectionString, containerName);
        }

        public string Setting(IComponentIdentity componentIdentity, string settingName)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            if (String.IsNullOrWhiteSpace(settingName)) throw new ArgumentNullException(nameof(settingName));

            return _configuration[_nameProvider.SettingName(componentIdentity, settingName)];
        }

        public string StorageAccountConnectionString(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            return _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
        }

        public string SqlConnectionString(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            return _applicationResourceSettingProvider.SqlConnectionString(componentIdentity);
        }
    }
}
