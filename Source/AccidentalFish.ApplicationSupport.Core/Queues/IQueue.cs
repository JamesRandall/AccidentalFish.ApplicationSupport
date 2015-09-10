using System;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Basic queue interface. This non-async version is likely to be deprecated.
    /// </summary>
    /// <typeparam name="T">Type of the queue item</typeparam>
    public interface IQueue<T> where T : class
    {
        /// <summary>
        /// Enqueue the given item and call success or failure handlers as appropriate
        /// </summary>
        /// <param name="item">The item to enqueue</param>
        /// <param name="success">Action to call on success</param>
        /// <param name="failure">Action to call on failure</param>
        void Enqueue(T item, Action<T> success = null, Action<T, Exception> failure = null);
        /// <summary>
        /// Dequeue an item calling success or failure actiosn as appopriate
        /// </summary>
        /// <param name="success">On successful dequeue the item is passed to this function, it should return true to remove the item from the queue, false to leave it their</param>
        /// <param name="failure">Called on failure to dequeue. If null then an exception is thrown.</param>
        void Dequeue(Func<IQueueItem<T>, bool> success, Action<Exception> failure = null);

        /// <summary>
        /// Dequeue an item calling success or failure actiosn as appopriate
        /// </summary>
        /// <param name="success">On successful dequeue the item is passed to this function, it should return true to remove the item from the queue, false to leave it their</param>
        /// <param name="noMessageAction">Called if their was no message on the queue</param>
        /// <param name="failure">Called on failure to dequeue. If null then an exception is thrown.</param>
        void Dequeue(Func<IQueueItem<T>, bool> success, Action noMessageAction, Action<Exception> failure = null);
        /// <summary>
        /// Extend the visibility lease of the given queue item. Some implementations may throw NotSupportedException.
        /// </summary>
        /// <param name="queueItem">The queue item who's lease is to be extended</param>
        /// <param name="visibilityTimeout">The timeout to increase the lease by</param>
        void ExtendLease(IQueueItem<T> queueItem, TimeSpan visibilityTimeout);
        /// <summary>
        /// Extend the visibility lease of the given queue item. Some implementations may throw NotSupportedException.
        /// </summary>
        /// <param name="queueItem">The queue item who's lease is to be extended</param>
        void ExtendLease(IQueueItem<T> queueItem);
    }
}
