using System;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IQueue<T> where T : class
    {
        void Enqueue(T item, Action<T> success, Action<T, Exception> failure);
        void Dequeue(Func<IQueueItem<T>, bool> success, Action<Exception> failure);
        void Dequeue(Func<IQueueItem<T>, bool> success, Action noMessageAction, Action<Exception> failure);
        void ExtendLease(IQueueItem<T> queueItem, TimeSpan visibilityTimeout);
    }
}
