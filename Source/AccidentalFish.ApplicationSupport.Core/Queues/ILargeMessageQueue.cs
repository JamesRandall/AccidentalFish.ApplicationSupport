using AccidentalFish.ApplicationSupport.Core.Blobs;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// A large message queue combines a blob store and a queue (with references to the blobs) to allow for the queueing
    /// of items of a greater size than typically supported by standard queues. The penalty for this is that each enqueue /
    /// dequeue operation requires two storage requests - one to access queue item containing the blob reference and another to
    /// upload / download the blob.
    /// 
    /// Large message queues are interchangeable with standard IAsynchronousQueue types as this interface simply
    /// extends the basic IAsynchronousQueue interface to expose the underlying blob repository and queue of
    /// a large message queue.
    /// </summary>
    /// <typeparam name="T">Type of queue message</typeparam>
    public interface ILargeMessageQueue<T> : IAsynchronousQueue<T> where T : class
    {
        /// <summary>
        /// The queue that contains references to the blobs
        /// </summary>
        IAsynchronousQueue<LargeMessageReference> ReferenceQueue { get; }

        /// <summary>
        /// The blob repository that contains the message
        /// </summary>
        IAsynchronousBlockBlobRepository BlobRepository { get; }
    }
}
