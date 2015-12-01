using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Queues.Model
{
    /// <summary>
    /// Represents a large message on a queue providing access to the underlying queue item and through that the blob
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LargeMessageQueueItem<T> : IQueueItem<T> where T : class
    {
        internal LargeMessageQueueItem(T item, int dequeueCount, IQueueItem<LargeMessageReference> actualQueueItem)
        {
            Item = item;
            DequeueCount = dequeueCount;
            ActualQueueItem = actualQueueItem;
        }

        /// <summary>
        /// The queue payload - deserialized from the blob
        /// </summary>
        public T Item { get; }
        /// <summary>
        /// Dequeue count - proxied from the reference queue item
        /// </summary>
        public int DequeueCount { get; }
        /// <summary>
        /// The actual, real, queue item with the blob reference
        /// </summary>
        public IQueueItem<LargeMessageReference> ActualQueueItem { get; }
        /// <summary>
        /// Message properties - proxied from the reference queue item
        /// </summary>
        public IReadOnlyDictionary<string, object> Properties => ActualQueueItem.Properties;
    }
}
