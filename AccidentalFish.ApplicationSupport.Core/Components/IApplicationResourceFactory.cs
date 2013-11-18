using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Repository;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public interface IApplicationResourceFactory
    {
        IUnitOfWorkFactory GetUnitOfWorkFactory(IComponentIdentity componentIdentity);

        IAsynchronousNoSqlRepository<T> GetNoSqlRepository<T>(IComponentIdentity componentIdentity) where T : NoSqlEntity, new();
        IAsynchronousNoSqlRepository<T> GetNoSqlRepository<T>(string tablename, IComponentIdentity componentIdentity) where T : NoSqlEntity, new();

        IAsynchronousQueue<T> GetQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;

        IAsynchronousBlockBlobRepository GetBlockBlobRepository(IComponentIdentity componentIdentity);
        IAsynchronousBlockBlobRepository GetBlockBlobRepository(string containerName, IComponentIdentity componentIdentity);

        string StorageAccountConnectionString(IComponentIdentity componentIdentity);
        string Setting(IComponentIdentity componentIdentity, string settingName);
    }
}
