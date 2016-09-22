using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Core.Components.Implementation
{
    internal class AsyncApplicationResourceFactory : IAsyncApplicationResourceFactory
    {
        private readonly IAsyncApplicationResourceSettingProvider _applicationResourceSettingProvider;
        private readonly IApplicationResourceSettingNameProvider _nameProvider;
        private readonly IQueueFactory _queueFactory;
        private readonly IBlobRepositoryFactory _blobRepositoryFactory;
        private readonly IAsyncConfiguration _configuration;
        private readonly ILeaseManagerFactory _leaseManagerFactory;
        private readonly IUnitOfWorkFactoryProvider _unitOfWorkFactoryProvider;
        private readonly ICoreAssemblyLogger _logger;

        public AsyncApplicationResourceFactory(
            IAsyncApplicationResourceSettingProvider applicationResourceSettingProvider,
            IApplicationResourceSettingNameProvider nameProvider,
            IQueueFactory queueFactory,
            IBlobRepositoryFactory blobRepositoryFactory,
            IAsyncConfiguration configuration,
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

        public async Task<IUnitOfWorkFactory> GetUnitOfWorkFactoryAsync(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            _logger?.Verbose("ApplicationResourceFactory - GetUnitOfWorkFactory - {0}", componentIdentity);

            string sqlConnectionString = await _applicationResourceSettingProvider.SqlConnectionStringAsync(componentIdentity);
            string contextType = await _applicationResourceSettingProvider.SqlContextTypeAsync(componentIdentity);
            return _unitOfWorkFactoryProvider.Create(contextType, sqlConnectionString);
        }

        public async Task<ILeaseManager<T>> GetLeaseManagerAsync<T>(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            _logger?.Verbose("ApplicationResourceFactory - GetLeaseManager - {0}", componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            string defaultLeaseBlockName = await _applicationResourceSettingProvider.DefaultLeaseBlockNameAsync(componentIdentity);
            return _leaseManagerFactory.CreateLeaseManager<T>(storageAccountConnectionString, defaultLeaseBlockName);
        }

        public async Task<ILeaseManager<T>> GetLeaseManagerAsync<T>(string leaseBlockName, IComponentIdentity componentIdentity)
        {
            if (String.IsNullOrWhiteSpace(leaseBlockName)) throw new ArgumentNullException(nameof(leaseBlockName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetLeaseManager - {0},{1}", leaseBlockName, componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            return _leaseManagerFactory.CreateLeaseManager<T>(storageAccountConnectionString, leaseBlockName);
        }

        public async Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncQueue - {0}", componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            string defaultQueueName = await _applicationResourceSettingProvider.DefaultQueueNameAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, defaultQueueName);
        }

        public async Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncQueue - {0},{1}", queuename, componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, queuename);
        }

        public async Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncQueue - {0}", componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            string defaultQueueName = await _applicationResourceSettingProvider.DefaultQueueNameAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, defaultQueueName, queueSerializer);
        }

        public async Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(string queuename, IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncQueue - {0},{1}", queuename, componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, queuename, queueSerializer);
        }

        public async Task<IQueue<T>> GetQueueAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetQueue - {0}", componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            string defaultQueueName = await _applicationResourceSettingProvider.DefaultQueueNameAsync(componentIdentity);
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, defaultQueueName);
        }

        public async Task<IQueue<T>> GetQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetQueue - {0},{1}", queuename, componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, queuename);
        }

        public async Task<IQueue<T>> GetQueueAsync<T>(IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetQueue - {0}", componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            string defaultQueueName = await _applicationResourceSettingProvider.DefaultQueueNameAsync(componentIdentity);
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, defaultQueueName, queueSerializer);
        }

        public async Task<IQueue<T>> GetQueueAsync<T>(string queuename, IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetQueue - {0},{1}", queuename, componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            return _queueFactory.CreateQueue<T>(storageAccountConnectionString, queuename, queueSerializer);
        }

        public async Task<IAsynchronousTopic<T>> GetAsyncTopicAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncTopic - {0}", componentIdentity);

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            string defaultTopicName = await _applicationResourceSettingProvider.DefaultTopicNameAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousTopic<T>(serviceBusConnectionString, defaultTopicName);
        }

        public async Task<IAsynchronousTopic<T>> GetAsyncTopicAsync<T>(string topicName, IComponentIdentity componentIdentity) where T : class
        {
            if (String.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncTopic - {0},{1}", topicName, componentIdentity);

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousTopic<T>(serviceBusConnectionString, topicName);
        }

        public async Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncSubscription - {0}", componentIdentity);

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            string defaultTopicName = await _applicationResourceSettingProvider.DefaultTopicNameAsync(componentIdentity);
            string defaultSubscriptionName = await _applicationResourceSettingProvider.DefaultSubscriptionNameAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousSubscription<T>(serviceBusConnectionString, defaultTopicName, defaultSubscriptionName);
        }

        public async Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class
        {
            if (String.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException(nameof(subscriptionName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncSubscription - {0},{1}", subscriptionName, componentIdentity);

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            string defaultTopicName = await _applicationResourceSettingProvider.DefaultTopicNameAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousSubscription<T>(serviceBusConnectionString, defaultTopicName, subscriptionName);
        }

        public async Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(string subscriptionName, string topicName, IComponentIdentity componentIdentity) where T : class
        {
            if (String.IsNullOrWhiteSpace(subscriptionName)) throw new ArgumentNullException(nameof(subscriptionName));
            if (String.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncSubscription - {0},{1},{2}", subscriptionName, topicName, componentIdentity);

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousSubscription<T>(serviceBusConnectionString, topicName, subscriptionName);
        }

        public async Task<IAsynchronousBlockBlobRepository> GetAsyncBlockBlobRepositoryAsync(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncBlockBlobRepository - {0}", componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            string blobContainerName = await _applicationResourceSettingProvider.DefaultBlobContainerNameAsync(componentIdentity);
            return _blobRepositoryFactory.CreateAsynchronousBlockBlobRepository(storageAccountConnectionString, blobContainerName);
        }

        public async Task<IAsynchronousBlockBlobRepository> GetAsyncBlockBlobRepositoryAsync(string containerName, IComponentIdentity componentIdentity)
        {
            if (String.IsNullOrWhiteSpace(containerName)) throw new ArgumentNullException(nameof(containerName));
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));

            _logger?.Verbose("ApplicationResourceFactory - GetAsyncBlockBlobRepository - {0},{1}", containerName, componentIdentity);

            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            return _blobRepositoryFactory.CreateAsynchronousBlockBlobRepository(storageAccountConnectionString, containerName);
        }

        public Task<string> SettingAsync(IComponentIdentity componentIdentity, string settingName)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            if (String.IsNullOrWhiteSpace(settingName)) throw new ArgumentNullException(nameof(settingName));

            return _configuration.GetAsync(_nameProvider.SettingName(componentIdentity, settingName));
        }

        public Task<string> StorageAccountConnectionStringAsync(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            return _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
        }

        public Task<string> SqlConnectionStringAsync(IComponentIdentity componentIdentity)
        {
            if (componentIdentity == null) throw new ArgumentNullException(nameof(componentIdentity));
            return _applicationResourceSettingProvider.SqlConnectionStringAsync(componentIdentity);
        }
    }
}
