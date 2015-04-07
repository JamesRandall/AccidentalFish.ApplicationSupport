using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Components.Implementation
{
    internal class AzureApplicationResourceFactory : IAzureApplicationResourceFactory
    {
        private readonly IApplicationResourceFactory _applicationResourceFactory;
        private readonly IApplicationResourceSettingProvider _applicationResourceSettingProvider;
        private readonly ITableStorageRepositoryFactory _tableStorageRepositoryFactory;

        public AzureApplicationResourceFactory(IApplicationResourceFactory applicationResourceFactory,
            IApplicationResourceSettingProvider applicationResourceSettingProvider,
            ITableStorageRepositoryFactory tableStorageRepositoryFactory)
        {
            _applicationResourceFactory = applicationResourceFactory;
            _applicationResourceSettingProvider = applicationResourceSettingProvider;
            _tableStorageRepositoryFactory = tableStorageRepositoryFactory;
        }

        #region Decorated methods

        public IUnitOfWorkFactory GetUnitOfWorkFactory(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetUnitOfWorkFactory(componentIdentity);
        }

        public ILeaseManager<T> GetLeaseManager<T>(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetLeaseManager<T>(componentIdentity);
        }

        public ILeaseManager<T> GetLeaseManager<T>(string leaseBlockName, IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetLeaseManager<T>(leaseBlockName, componentIdentity);
        }

        public IAsynchronousQueue<T> GetAsyncQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncQueue<T>(componentIdentity);
        }

        public IAsynchronousQueue<T> GetAsyncQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncQueue<T>(queuename, componentIdentity);
        }

        public IAsynchronousQueue<T> GetAsyncBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncBrokeredMessageQueue<T>(componentIdentity);
        }

        public IAsynchronousQueue<T> GetAsyncBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncBrokeredMessageQueue<T>(queuename, componentIdentity);
        }

        public IQueue<T> GetQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetQueue<T>(componentIdentity);
        }

        public IQueue<T> GetQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetQueue<T>(queuename, componentIdentity);
        }

        public IQueue<T> GetBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetBrokeredMessageQueue<T>(componentIdentity);
        }

        public IQueue<T> GetBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetBrokeredMessageQueue<T>(queuename, componentIdentity);
        }

        public IAsynchronousTopic<T> GetAsyncTopic<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncTopic<T>(componentIdentity);
        }

        public IAsynchronousTopic<T> GetAsyncTopic<T>(string topicName, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncTopic<T>(topicName, componentIdentity);
        }

        public IAsynchronousSubscription<T> GetAsyncSubscription<T>(IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncSubscription<T>(componentIdentity);
        }

        public IAsynchronousSubscription<T> GetAsyncSubscription<T>(string subscriptionName, IComponentIdentity componentIdentity) where T : class
        {
            return _applicationResourceFactory.GetAsyncSubscription<T>(subscriptionName, componentIdentity);
        }

        public IAsynchronousBlockBlobRepository GetAsyncBlockBlobRepository(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetAsyncBlockBlobRepository(componentIdentity);
        }

        public IAsynchronousBlockBlobRepository GetAsyncBlockBlobRepository(string containerName, IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.GetAsyncBlockBlobRepository(containerName, componentIdentity);
        }

        public string StorageAccountConnectionString(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.StorageAccountConnectionString(componentIdentity);
        }

        public string SqlConnectionString(IComponentIdentity componentIdentity)
        {
            return _applicationResourceFactory.SqlConnectionString(componentIdentity);
        }

        public string Setting(IComponentIdentity componentIdentity, string settingName)
        {
            return _applicationResourceFactory.Setting(componentIdentity, settingName);
        }

        #endregion

        public IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(IComponentIdentity componentIdentity) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            string defaultTableName = _applicationResourceSettingProvider.DefaultTableName(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, defaultTableName);
        }

        public IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(string tablename, IComponentIdentity componentIdentity) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tablename);
        }

        public IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(string tablename, IComponentIdentity componentIdentity,
            bool lazyCreateTable) where T : ITableEntity, new()
        {
            string storageAccountConnectionString = _applicationResourceSettingProvider.StorageAccountConnectionString(componentIdentity);
            return _tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tablename, lazyCreateTable);
        }
    }
}
