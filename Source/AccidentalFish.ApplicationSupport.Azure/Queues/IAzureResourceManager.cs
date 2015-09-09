using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    public interface IAzureResourceManager
    {
        Task CreateIfNotExistsAsync<T>(IAsynchronousQueue<T> queue) where T : class;

        void CreateIfNotExists<T>(IQueue<T> queue) where T : class;
    }
}
