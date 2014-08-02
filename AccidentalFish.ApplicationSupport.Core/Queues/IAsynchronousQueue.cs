using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IAsynchronousQueue<T> where T : class
    {
        Task EnqueueAsync(T item);
        Task EnqueueAsync(T item, TimeSpan initialVisibilityDelay);
        void Enqueue(T item, Action<T> success, Action<T, Exception> failure);
        Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor);
        Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor, TimeSpan? visibilityTimeout);
        void Dequeue(Func<IQueueItem<T>, bool> success, Action<Exception> failure);
        void Dequeue(Func<IQueueItem<T>, bool> success, Action noMessageAction, Action<Exception> failure);
        Task ExtendLeaseAsync(IQueueItem<T> queueItem, TimeSpan visibilityTimeout);
    }
}
