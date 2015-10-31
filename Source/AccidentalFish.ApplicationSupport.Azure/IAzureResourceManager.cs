using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure
{
    public interface IAzureResourceManager
    {
        Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousQueue<T> queue) where T : class;
        Task<bool> DeleteIfExistsAsync<T>(IAsynchronousQueue<T> queue) where T : class;

        bool CreateIfNotExists<T>(IQueue<T> queue) where T : class;
        bool DeleteIfExists<T>(IQueue<T> queue) where T : class;

        Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousTopic<T> topic) where T : class;
        Task<bool> DeleteIfExistsAsync<T>(IAsynchronousTopic<T> topic) where T : class;

        Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousSubscription<T> subscription) where T : class;
        Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousSubscription<T> subscription, Filter filter) where T : class;
        Task<bool> DeleteIfExistsAsync<T>(IAsynchronousSubscription<T> subscription) where T : class;

        Task<bool> CreateIfNotExistsAsync<T>(IAsynchronousTableStorageRepository<T> table) where T : ITableEntity, new();
        Task<bool> DeleteIfExistsAsync<T>(IAsynchronousTableStorageRepository<T> table) where T : ITableEntity, new();

        Task<bool> CreateIfNotExistsAsync(IAsynchronousBlockBlobRepository repository);
        Task<bool> DeleteIfExistsAsync(IAsynchronousBlockBlobRepository repository);
    }
}
