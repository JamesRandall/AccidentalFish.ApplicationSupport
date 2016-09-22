using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Queues;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Components.Implementation
{
    internal class AsyncAzureApplicationResourceFactory : IAsyncAzureApplicationResourceFactory
    {
        private readonly IAsyncApplicationResourceFactory _applicationResourceFactory;
        private readonly IAsyncApplicationResourceSettingProvider _applicationResourceSettingProvider;
        private readonly ITableStorageRepositoryFactory _tableStorageRepositoryFactory;
        private readonly IAzureQueueFactory _queueFactory;

        public AsyncAzureApplicationResourceFactory(IAsyncApplicationResourceFactory applicationResourceFactory,
            IAsyncApplicationResourceSettingProvider applicationResourceSettingProvider,
            ITableStorageRepositoryFactory tableStorageRepositoryFactory,
            IAzureQueueFactory queueFactory)
        {
            _applicationResourceFactory = applicationResourceFactory;
            _applicationResourceSettingProvider = applicationResourceSettingProvider;
            _tableStorageRepositoryFactory = tableStorageRepositoryFactory;
            _queueFactory = queueFactory;
        }

        #region Decorated methods

        public Task<IUnitOfWorkFactory> GetUnitOfWorkFactoryAsync(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetUnitOfWorkFactoryAsync(componentIdentity);
        }

        public Task<ILeaseManager<T>> GetLeaseManagerAsync<T>(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetLeaseManagerAsync<T>(componentIdentity);
        }

        public Task<ILeaseManager<T>> GetLeaseManagerAsync<T>(string leaseBlockName, IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetLeaseManagerAsync<T>(leaseBlockName, componentIdentity);
        }

        public Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncQueueAsync<T>(componentIdentity);
        }

        public Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncQueueAsync<T>(queuename, componentIdentity);
        }

        public Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            return _applicationResourceFactory.GetAsyncQueueAsync<T>(componentIdentity, queueSerializer);
        }

        public Task<IAsynchronousQueue<T>> GetAsyncQueueAsync<T>(string queuename, IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            return _applicationResourceFactory.GetAsyncQueueAsync<T>(queuename, componentIdentity, queueSerializer);
        }

        public Task<IQueue<T>> GetQueueAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetQueueAsync<T>(componentIdentity);
        }

        public Task<IQueue<T>> GetQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetQueueAsync<T>(queuename, componentIdentity);
        }

        public Task<IQueue<T>> GetQueueAsync<T>(IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            return _applicationResourceFactory.GetQueueAsync<T>(componentIdentity, queueSerializer);
        }

        public Task<IQueue<T>> GetQueueAsync<T>(string queuename, IComponentIdentity componentIdentity, IQueueSerializer queueSerializer) where T : class
        {
            return _applicationResourceFactory.GetQueueAsync<T>(queuename, componentIdentity, queueSerializer);
        }

        public Task<IAsynchronousTopic<T>> GetAsyncTopicAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncTopicAsync<T>(componentIdentity);
        }

        public Task<IAsynchronousTopic<T>> GetAsyncTopicAsync<T>(string topicName, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncTopicAsync<T>(topicName, componentIdentity);
        }

        public Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncSubscriptionAsync<T>(componentIdentity);
        }

        public Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncSubscriptionAsync<T>(subscriptionName, componentIdentity);
        }

        public Task<IAsynchronousSubscription<T>> GetAsyncSubscriptionAsync<T>(string subscriptionName, string topicName,
            IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncSubscriptionAsync<T>(subscriptionName, topicName, componentIdentity);
        }

        public Task<IAsynchronousBlockBlobRepository> GetAsyncBlockBlobRepositoryAsync(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetAsyncBlockBlobRepositoryAsync(componentIdentity);
        }

        public Task<IAsynchronousBlockBlobRepository> GetAsyncBlockBlobRepositoryAsync(string containerName, IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetAsyncBlockBlobRepositoryAsync(containerName, componentIdentity);
        }

        public Task<string> StorageAccountConnectionStringAsync(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.StorageAccountConnectionStringAsync(componentIdentity);
        }

        public Task<string> SqlConnectionStringAsync(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.SqlConnectionStringAsync(componentIdentity);
        }

        public Task<string> SettingAsync(IComponentIdentity componentIdentity, string settingName)
        {
            return _applicationResourceFactory.SettingAsync(componentIdentity, settingName);
        }

        #endregion

        public async Task<IAsynchronousTableStorageRepository<T>> GetTableStorageRepositoryAsync<T>(IComponentIdentity componentIdentity) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            string defaultTableName = await _applicationResourceSettingProvider.DefaultTableNameAsync(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, defaultTableName);
        }

        public async Task<IAsynchronousTableStorageRepository<T>> GetTableStorageRepositoryAsync<T>(string tablename, IComponentIdentity componentIdentity) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tablename);
        }

        public async Task<IAsynchronousTableStorageRepository<T>> GetTableStorageRepositoryAsync<T>(string tablename, IComponentIdentity componentIdentity,
            bool lazyCreateTable) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = await _applicationResourceSettingProvider.StorageAccountConnectionStringAsync(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tablename, lazyCreateTable);
        }

        public async Task<IAsynchronousQueue<T>> GetAsyncBrokeredMessageQueueAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException("componentIdentity");

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            string defaultQueueName = await _applicationResourceSettingProvider.DefaultBrokeredMessageQueueNameAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousBrokeredMessageQueue<T>(serviceBusConnectionString, defaultQueueName);
        }

        public async Task<IAsynchronousQueue<T>> GetAsyncBrokeredMessageQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            if (String.IsNullOrWhiteSpace(queuename)) throw new ArgumentNullException("queuename");
            if (componentIdentity == null) throw new ArgumentNullException("componentIdentity");

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            return _queueFactory.CreateAsynchronousBrokeredMessageQueue<T>(serviceBusConnectionString, queuename);
        }

        public async Task<IQueue<T>> GetBrokeredMessageQueueAsync<T>(IComponentIdentity componentIdentity) where T : class
        {
            if (componentIdentity == null) throw new ArgumentNullException("componentIdentity");

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            string defaultQueueName = await _applicationResourceSettingProvider.DefaultBrokeredMessageQueueNameAsync(componentIdentity);
            return _queueFactory.CreateBrokeredMessageQueue<T>(serviceBusConnectionString, defaultQueueName);
        }

        public async Task<IQueue<T>> GetBrokeredMessageQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            if (String.IsNullOrWhiteSpace(queuename)) throw new ArgumentNullException("queuename");
            if (componentIdentity == null) throw new ArgumentNullException("componentIdentity");

            string serviceBusConnectionString = await _applicationResourceSettingProvider.ServiceBusConnectionStringAsync(componentIdentity);
            return _queueFactory.CreateBrokeredMessageQueue<T>(serviceBusConnectionString, queuename);
        }
    }
}
