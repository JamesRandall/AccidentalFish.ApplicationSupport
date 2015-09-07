using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    public interface IAsynchronousStorageQueue<T> : IAsynchronousQueue<T> where T : class
    {
        Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor, TimeSpan? visibilityTimeout);
    }
}
