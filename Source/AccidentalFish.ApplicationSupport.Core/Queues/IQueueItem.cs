using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Wrapper for a queued item
    /// </summary>
    /// <typeparam name="T">The type of the queue payload</typeparam>
    public interface IQueueItem<out T> where T : class
    {
        /// <summary>
        /// The queue payload
        /// </summary>
        T Item { get; }

        /// <summary>
        /// The number of times the item has been dequeued
        /// </summary>
        int DequeueCount { get; }

        /// <summary>
        /// Properties associated with the message
        /// </summary>
        IReadOnlyDictionary<string, object> Properties { get; }
    }
}
