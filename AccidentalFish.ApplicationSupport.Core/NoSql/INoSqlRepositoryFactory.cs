namespace AccidentalFish.ApplicationSupport.Core.NoSql
{
    public interface INoSqlRepositoryFactory
    {
        IAsynchronousNoSqlRepository<T> CreateAsynchronousNoSqlRepository<T>(string storageAccountConnectionString, string tableName, bool lazyTableCreation) where T : NoSqlEntity, new();
        IAsynchronousNoSqlRepository<T> CreateAsynchronousNoSqlRepository<T>(string storageAccountConnectionString, string tableName) where T : NoSqlEntity, new();
        IAsynchronousNoSqlRepository<T> CreateAsynchronousNoSqlRepository<T>(string tableName) where T : NoSqlEntity, new();
        ILeaseManager<T> CreateLeaseManager<T>(string storageAccountConnectionString, string leaseBlockName);
    }
}
