using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure
{
    public interface IAzureResourceManager
    {
        Task CreateIfNotExistsAsync<T>(IAsynchronousQueue<T> queue) where T : class;
        Task DeleteIfExistsAsync<T>(IAsynchronousQueue<T> queue) where T : class;

        void CreateIfNotExists<T>(IQueue<T> queue) where T : class;
        void DeleteIfExists<T>(IQueue<T> queue) where T : class;

        Task CreateIfNotExistsAsync<T>(IAsynchronousTopic<T> topic) where T : class;
        Task DeleteIfExistsAsync<T>(IAsynchronousTopic<T> topic) where T : class;

        Task CreateIfNotExistsAsync<T>(IAsynchronousSubscription<T> subscription) where T : class;
        Task DeleteIfExistsAsync<T>(IAsynchronousSubscription<T> subscription) where T : class;

        Task CreateIfNotExistsAsync<T>(IAsynchronousTableStorageRepository<T> table) where T : ITableEntity, new();
        Task DeleteIfExistsAsync<T>(IAsynchronousTableStorageRepository<T> table) where T : ITableEntity, new();
    }
}
