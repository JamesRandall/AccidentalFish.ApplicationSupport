using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Components
{
    public interface IAzureApplicationResourceFactory : IApplicationResourceFactory
    {
        IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(IComponentIdentity componentIdentity) where T : ITableEntity, new();
        IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(string tablename, IComponentIdentity componentIdentity) where T : ITableEntity, new();
        IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(string tablename, IComponentIdentity componentIdentity, bool lazyCreateTable) where T : ITableEntity, new();
        IQueue<T> GetBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IQueue<T> GetBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetAsyncBrokeredMessageQueue<T>(IComponentIdentity componentIdentity) where T : class;
        IAsynchronousQueue<T> GetAsyncBrokeredMessageQueue<T>(string queuename, IComponentIdentity componentIdentity) where T : class;
    }
}
