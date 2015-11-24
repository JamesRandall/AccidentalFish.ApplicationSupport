using System;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using static System.String;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation
{
    internal class TableStorageRepositoryFactory : ITableStorageRepositoryFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ITableStorageQueryBuilder _tableStorageQueryBuilder;
        private readonly ITableContinuationTokenSerializer _tableContinuationTokenSerializer;
        private readonly IAzureResourceManager _azureResourceManager;
        private readonly IAzureAssemblyLogger _logger;

        public TableStorageRepositoryFactory(
            IConfiguration configuration,
            ITableStorageQueryBuilder tableStorageQueryBuilder,
            ITableContinuationTokenSerializer tableContinuationTokenSerializer,
            IAzureResourceManager azureResourceManager,
            IAzureAssemblyLogger logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (tableStorageQueryBuilder == null) throw new ArgumentNullException(nameof(tableStorageQueryBuilder));
            if (tableContinuationTokenSerializer == null) throw new ArgumentNullException(nameof(tableContinuationTokenSerializer));

            _configuration = configuration;
            _tableStorageQueryBuilder = tableStorageQueryBuilder;
            _tableContinuationTokenSerializer = tableContinuationTokenSerializer;
            _azureResourceManager = azureResourceManager;
            _logger = logger;
        }

        public IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(
            string storageAccountConnectionString,
            string tableName) where T : ITableEntity, new()
        {
            return CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tableName, false);
        }

        public IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(string tableName) where T : ITableEntity, new()
        {
            if (IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException(nameof(tableName));

            return new AsynchronousTableStorageRepository<T>(_configuration.StorageAccountConnectionString, tableName, _tableStorageQueryBuilder, _tableContinuationTokenSerializer, _logger);
        }

        public IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(
            string storageAccountConnectionString, string tableName, bool lazyTableCreation)
            where T : ITableEntity, new()
        {
            if (lazyTableCreation)
            {
                return new LazyTableCreateAsynchronousTableStorageRepository<T>(new AsynchronousTableStorageRepository<T>(storageAccountConnectionString, tableName, _tableStorageQueryBuilder, _tableContinuationTokenSerializer, _logger), _azureResourceManager);
            }
            return new AsynchronousTableStorageRepository<T>(storageAccountConnectionString, tableName, _tableStorageQueryBuilder, _tableContinuationTokenSerializer, _logger);
        }
    }
}
