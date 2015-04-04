using AccidentalFish.ApplicationSupport.Azure.NoSql;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using CuttingEdge.Conditions;
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
            Condition.Requires(configuration).IsNotNull();
            _configuration = configuration;
            _tableStorageQueryBuilder = tableStorageQueryBuilder;
            _tableContinuationTokenSerializer = tableContinuationTokenSerializer;
        }

        public IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(
            string storageAccountConnectionString,
            string tableName) where T : ITableEntity, new()
        {
            Condition.Requires(storageAccountConnectionString).IsNotNullOrWhiteSpace();
            Condition.Requires(tableName).IsNotNullOrWhiteSpace();
            return CreateAsynchronousNoSqlRepository<T>(storageAccountConnectionString, tableName, false);
        }

        public IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(string tableName) where T : ITableEntity, new()
        {
            Condition.Requires(tableName).IsNotNullOrWhiteSpace();
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
            else
            {
                return new AsynchronousTableStorageRepository<T>(storageAccountConnectionString, tableName, _tableStorageQueryBuilder, _tableContinuationTokenSerializer);
            }
        }
    }
}
