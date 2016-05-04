using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Allows errors to be handeld when errors occur within large message blob handling to enable the reference queue item
    /// to be removed or retained as appropriate.
    /// </summary>
    public interface ILargeMessageQueueErrorHandler
    {
        /// <summary>
        /// Called when a blob cannot be downloaded. Returning true will remove the reference item from the queue.
        /// </summary>
        /// <param name="ex">The underlying exception.</param>
        /// <param name="queueItem">The reference queue item.</param>
        Task<bool> UnableToDownloadBlobAsync(Exception ex, IQueueItem<LargeMessageReference> queueItem);
        /// <summary>
        /// Called when a blob cannot be downloaded. Returning true will remove the reference item from the queue.
        /// </summary>
        /// <param name="ex">The underlying exception.</param>
        /// <param name="queueItem">The reference queue item.</param>
        Task<bool> UnableToDeleteBlobAsync(Exception ex, IQueueItem<LargeMessageReference> queueItem);
    }
}
