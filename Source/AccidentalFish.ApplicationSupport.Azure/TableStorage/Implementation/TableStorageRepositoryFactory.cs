using System;
using AccidentalFish.ApplicationSupport.Azure.NoSql;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation
{
    internal class TableStorageRepositoryFactory : ITableStorageRepositoryFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ITableStorageQueryBuilder _tableStorageQueryBuilder;
        private readonly ITableContinuationTokenSerializer _tableContinuationTokenSerializer;

        public TableStorageRepositoryFactory(
            IConfiguration configuration,
            ITableStorageQueryBuilder tableStorageQueryBuilder,
            ITableContinuationTokenSerializer tableContinuationTokenSerializer)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (tableStorageQueryBuilder == null) throw new ArgumentNullException("tableStorageQueryBuilder");
            if (tableContinuationTokenSerializer == null) throw new ArgumentNullException("tableContinuationTokenSerializer");

            _configuration = configuration;
            _tableStorageQueryBuilder = tableStorageQueryBuilder;
            _tableContinuationTokenSerializer = tableContinuationTokenSerializer;
        }

        public IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(
            string storageAccountConnectionString,
            string tableName) where T : ITableEntity, new()
        {
            return CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tableName, false);
        }

        public IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(string tableName) where T : ITableEntity, new()
        {
            if (String.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException("tableName");

            return new AsynchronousTableStorageRepository<T>(_configuration.StorageAccountConnectionString, tableName, _tableStorageQueryBuilder, _tableContinuationTokenSerializer);
        }

        public IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(
            string storageAccountConnectionString, string tableName, bool lazyTableCreation)
            where T : ITableEntity, new()
        {
            if (lazyTableCreation)
            {
                return new LazyTableCreateAsynchronousTableStorageRepository<T>(new AsynchronousTableStorageRepository<T>(storageAccountConnectionString, tableName, _tableStorageQueryBuilder, _tableContinuationTokenSerializer));
            }
            return new AsynchronousTableStorageRepository<T>(storageAccountConnectionString, tableName, _tableStorageQueryBuilder, _tableContinuationTokenSerializer);
        }
    }
}
