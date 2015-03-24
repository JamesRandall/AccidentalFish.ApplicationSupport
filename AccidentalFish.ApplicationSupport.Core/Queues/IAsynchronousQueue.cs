using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    // TODO: We need to break this out into a simple base interface and then one with extensions to deal
    // with differences between queue types but still have a single common root interface with basic
    // enqueue and dequeue functionality
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
