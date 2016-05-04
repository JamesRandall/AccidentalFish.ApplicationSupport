using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Factory for creating large message queues. Large message queues are not limited to the size constraints of Azure service bus and storage queues
    /// as they use a blob store for their message payload.
    /// </summary>
    public interface ILargeMessageQueueFactory
    {
        /// <summary>
        /// Create a large message queue for a named component.
        /// The componentes default queue and blob store will be used.
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="componentIdentity"></param>
        /// <returns></returns>
        ILargeMessageQueue<T> Create<T>(IComponentIdentity componentIdentity) where T : class;

        /// <summary>
        /// Create a large message queue for a named component.
        /// The componentes default queue and blob store will be used.
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="componentIdentity">Identity of the component containing configuration information</param>
        /// <param name="errorHandler">Optional error handler for blob errors</param>
        /// <returns></returns>
        ILargeMessageQueue<T> Create<T>(IComponentIdentity componentIdentity, ILargeMessageQueueErrorHandler errorHandler) where T : class;

        /// <summary>
        /// Create a large message queue
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="connectionString">Connection string for the queue and blob repository</param>
        /// <param name="queueName">Name of the queue</param>
        /// <param name="blobContainerName">Name of the blob container</param>
        /// <returns>A queue</returns>
        ILargeMessageQueue<T> Create<T>(string connectionString, string queueName, string blobContainerName) where T : class;
        /// <summary>
        /// Create a large message queue
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="connectionString">Connection string for the queue and blob repository</param>
        /// <param name="queueName">Name of the queue</param>
        /// <param name="blobContainerName">Name of the blob container</param>
        /// <param name="errorHandler">Optional error handler for blob errors</param>
        /// <returns>A queue</returns>
        ILargeMessageQueue<T> Create<T>(string connectionString, string queueName, string blobContainerName, ILargeMessageQueueErrorHandler errorHandler) where T : class;

        /// <summary>
        /// Create a large message queue
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="referenceQueue">The underlying queue that contains the blob messages</param>
        /// <param name="blobRepository"></param>
        /// <returns>A queue</returns>
        ILargeMessageQueue<T> Create<T>(
            IAsynchronousQueue<LargeMessageReference> referenceQueue,
            IAsynchronousBlockBlobRepository blobRepository) where T : class;

        /// <summary>
        /// Create a large message queue
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="referenceQueue">The underlying queue that contains the blob messages</param>
        /// <param name="blobRepository"></param>
        /// <param name="errorHandler">Optional error handler for blob errors</param>
        /// <returns>A queue</returns>
        ILargeMessageQueue<T> Create<T>(
            IAsynchronousQueue<LargeMessageReference> referenceQueue,
            IAsynchronousBlockBlobRepository blobRepository,
            ILargeMessageQueueErrorHandler errorHandler) where T : class;
    }
}
