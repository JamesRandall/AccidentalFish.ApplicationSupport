using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;
using CuttingEdge.Conditions;

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

        public ApplicationResourceFactory(
            IApplicationResourceSettingProvider applicationResourceSettingProvider,
            IApplicationResourceSettingNameProvider nameProvider,
            IQueueFactory queueFactory,
            IBlobRepositoryFactory blobRepositoryFactory,
            IConfiguration configuration,
            ILeaseManagerFactory leaseManagerFactory,
            IUnitOfWorkFactoryProvider unitOfWorkFactoryProvider)
        {
            _applicationResourceSettingProvider = applicationResourceSettingProvider;
            _nameProvider = nameProvider;
            _queueFactory = queueFactory;
            _blobRepositoryFactory = blobRepositoryFactory;
            _configuration = configuration;
            _leaseManagerFactory = leaseManagerFactory;
            _unitOfWorkFactoryProvider = unitOfWorkFactoryProvider;
        }

        public IUnitOfWorkFactory GetUnitOfWorkFactory(IComponentIdentity componentIdentity)
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string sqlConnectionString = _applicationResourceSettingProvider.SqlConnectionString(componentIdentity);
            string contextType = _applicationResourceSettingProvider.SqlContextType(componentIdentity);
            return _unitOfWorkFactoryProvider.Create(contextType, sqlConnectionString);
        }

        public ILeaseManager<T> GetLeaseManager<T>(IComponentIdentity componentIdentity)
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultLeaseBlockName = _applicationResourceSettingProvider.DefaultLeaseBlockName(componentIdentity);
            return _leaseManagerFactory.CreateLeaseManager<T>(storageAccountConnectionString, defaultLeaseBlockName);
        }

        public ILeaseManager<T> GetLeaseManager<T>(string leaseBlockName, IComponentIdentity componentIdentity)
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _leaseManagerFactory.CreateLeaseManager<T>(storageAccountConnectionString, leaseBlockName);
        }

        public IAsynchronousQueue<T> GetQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            Condition.Requires(componentIdentity).IsNotNull();
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultQueueName = _applicationResourceSettingProvider.DefaultQueueName(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, defaultQueueName);
        }

        public IAsynchronousQueue<T> GetQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _queueFactory.CreateAsynchronousQueue<T>(storageAccountConnectionString, queuename);
        }

        public IAsynchronousQueue<T> GetBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            string defaultQueueName = _applicationResourceSettingProvider.DefaultBrokeredMessageQueueName(componentIdentity);
            return _queueFactory.CreateAsynchronousBrokeredMessageQueue<T>(serviceBusConnectionString, defaultQueueName);
        }

        public IAsynchronousQueue<T> GetBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            Condition.Requires(queuename).IsNotNullOrWhiteSpace("queue name must be supplied");
            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            return _queueFactory.CreateAsynchronousBrokeredMessageQueue<T>(serviceBusConnectionString, queuename);
        }

        public IAsynchronousTopic<T> GetTopic<T>(IComponentIdentity componentIdentity) where T : class
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            string defaultTopicName = _applicationResourceSettingProvider.DefaultTopicName(componentIdentity);
            return _queueFactory.CreateAsynchronousTopic<T>(serviceBusConnectionString, defaultTopicName);
        }

        public IAsynchronousTopic<T> GetTopic<T>(string topicName, IComponentIdentity componentIdentity) where T : class
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            return _queueFactory.CreateAsynchronousTopic<T>(serviceBusConnectionString, topicName);
        }

        public IAsynchronousSubscription<T> GetSubscription<T>(IComponentIdentity componentIdentity) where T : class
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            string defaultTopicName = _applicationResourceSettingProvider.DefaultTopicName(componentIdentity);
            string defaultSubscriptionName = _applicationResourceSettingProvider.DefaultSubscriptionName(componentIdentity);
            return _queueFactory.CreateAsynchronousSubscription<T>(serviceBusConnectionString, defaultTopicName, defaultSubscriptionName);
        }

        public IAsynchronousSubscription<T> GetSubscription<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string serviceBusConnectionString = _applicationResourceSettingProvider.ServiceBusConnectionString(componentIdentity);
            string defaultTopicName = _applicationResourceSettingProvider.DefaultTopicName(componentIdentity);
            return _queueFactory.CreateAsynchronousSubscription<T>(serviceBusConnectionString, defaultTopicName, subscriptionName);
        }

        public IAsynchronousBlockBlobRepository GetBlockBlobRepository(IComponentIdentity componentIdentity)
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string blobContainerName = _applicationResourceSettingProvider.DefaultBlobContainerName(componentIdentity);
            return _blobRepositoryFactory.CreateAsynchronousBlockBlobRepository(storageAccountConnectionString, blobContainerName);
        }

        public IAsynchronousBlockBlobRepository GetBlockBlobRepository(string containerName, IComponentIdentity componentIdentity)
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            Condition.Requires(containerName).IsNotNullOrWhiteSpace("container name must be supplied");
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _blobRepositoryFactory.CreateAsynchronousBlockBlobRepository(storageAccountConnectionString, containerName);
        }

        public string Setting(IComponentIdentity componentIdentity, string settingName)
        {
            Condition.Requires(componentIdentity).IsNotNull("component identity must be given");
            Condition.Requires(settingName).IsNotNullOrWhiteSpace("setting name must be supplied");
            return _configuration[_nameProvider.SettingName(componentIdentity, settingName)];
        }

        public string StorageAccountConnectionString(IComponentIdentity componentIdentity)
        {
            return _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
        }

        public string SqlConnectionString(IComponentIdentity componentIdentity)
        {
            return _applicationResourceSettingProvider.SqlConnectionString(componentIdentity);
        }
    }
}
