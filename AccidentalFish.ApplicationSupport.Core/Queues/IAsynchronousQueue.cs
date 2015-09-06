using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    // TODO: We need to break this out into a simple base interface and then one with extensions to deal
    // with differences between queue types but still have a single common root interface with basic
    // enqueue and dequeue functionality
    /// <summary>
    /// Queue operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsynchronousQueue<T> where T : class
    {
        /// <summary>
        /// Enqueue the given item
        /// </summary>
        /// <param name="item">The item to queue</param>
        /// <returns>An awaitable task</returns>
        Task EnqueueAsync(T item);
        /// <summary>
        /// Enqueue an item and specify after how long until it appears in the queue.
        /// Implementations may throw a NotSupportedException if not supported.
        /// </summary>
        /// <param name="item">The item to queue</param>
        /// <param name="initialVisibilityDelay">The delay until the item appears in the queue</param>
        /// <returns>An awaitable task</returns>
        Task EnqueueAsync(T item, TimeSpan initialVisibilityDelay);
        /// <summary>
        /// Dequeues an item and passes it to the specified asynchronous function for processing
        /// </summary>
        /// <param name="processor">A function for processing the queue item. The function is passed the queue item and should return
        /// true if it wants the message to be removed / deleted from the queue, false if it it should be left on the queue (for example
        /// an error scenario)</param>
        /// <returns>An awaitable task that completes when the queue processing is complete</returns>
        Task DequeueAsync(Func<IQueueItem<T>, Task<bool>> processor);
        /// <summary>
        /// Where supported extends the amount of time available to process the queue item.
        /// Implementations may throw a NotSupportedException if not supported.
        /// </summary>
        /// <param name="queueItem">The queue item who's lease is to bextended</param>
        /// <param name="visibilityTimeout">The amount of time it is to be extended by.</param>
        /// <returns>An awaitable task.</returns>
        Task ExtendLeaseAsync(IQueueItem<T> queueItem, TimeSpan visibilityTimeout);
        /// <summary>
        /// Where supported extends the amount of time available to process the queue item.
        /// Implementations may throw a NotSupportedException if not supported.
        /// </summary>
        /// <param name="queueItem">The queue item who's lease is to be extended</param>
        /// <returns>An awaitable task.</returns>
        Task ExtendLeaseAsync(IQueueItem<T> queueItem);
    }
}
