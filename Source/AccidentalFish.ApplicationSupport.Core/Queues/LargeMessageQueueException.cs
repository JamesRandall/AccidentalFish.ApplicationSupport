using System;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Exception raised by large message queues
    /// </summary>
    public class LargeMessageQueueException : Exception
    {
        private readonly IQueueItem<LargeMessageReference> _queueItem;
        private readonly string _blobReference;
        
        internal LargeMessageQueueException(string message, Exception innerException, IQueueItem<LargeMessageReference> queueItem) : base(message, innerException)
        {
            _queueItem = queueItem;
        }

        internal LargeMessageQueueException(string message, Exception innerException, string blobReference) : base(message, innerException)
        {
            _blobReference = blobReference;
        }

        /// <summary>
        /// Reference to the blob
        /// </summary>
        public string BlobReference => _queueItem?.Item?.BlobReference ?? _blobReference;

        /// <summary>
        /// Reference to the queue item
        /// </summary>
        public IQueueItem<LargeMessageReference> QueueItem => _queueItem;
    }
}
