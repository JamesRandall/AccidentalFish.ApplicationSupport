using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage
{
    public interface ITableStorageRepositoryFactory
    {
        IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(string storageAccountConnectionString, string tableName, bool lazyTableCreation) where T : ITableEntity, new();
        IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(string storageAccountConnectionString, string tableName) where T : ITableEntity, new();
        IAsynchronousTableStorageRepository<T> CreateAsynchronousNoSqlRepository<T>(string tableName) where T : ITableEntity, new();
    }
}
