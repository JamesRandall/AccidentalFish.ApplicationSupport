using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Components
{
    public interface IAsyncAzureApplicationResourceFactory : IAsyncApplicationResourceFactory
    {
        Task<IAsynchronousTableStorageRepository<T>> GetTableStorageRepositoryAsync<T>(IComponentIdentity componentIdentity) where T : ITableEntity, new();
        Task<IAsynchronousTableStorageRepository<T>> GetTableStorageRepositoryAsync<T>(string tablename, IComponentIdentity componentIdentity) where T : ITableEntity, new();
        Task<IAsynchronousTableStorageRepository<T>> GetTableStorageRepositoryAsync<T>(string tablename, IComponentIdentity componentIdentity, bool lazyCreateTable) where T : ITableEntity, new();
        Task<IQueue<T>> GetBrokeredMessageQueueAsync<T>(IComponentIdentity componentIdentity) where T : class;
        Task<IQueue<T>> GetBrokeredMessageQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class;
        Task<IAsynchronousQueue<T>> GetAsyncBrokeredMessageQueueAsync<T>(IComponentIdentity componentIdentity) where T : class;
        Task<IAsynchronousQueue<T>> GetAsyncBrokeredMessageQueueAsync<T>(string queuename, IComponentIdentity componentIdentity) where T : class;
    }
}
