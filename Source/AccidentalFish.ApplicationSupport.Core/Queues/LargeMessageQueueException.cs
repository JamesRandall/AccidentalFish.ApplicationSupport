using System;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Exception raised by large message queues
    /// </summary>
    public class LargeMessageQueueException : Exception
    {
        private readonly string _blobReference;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        /// <param name="blobReference">Reference to the blob</param>
        public LargeMessageQueueException(string message, Exception innerException, string blobReference) : base(message, innerException)
        {
            _blobReference = blobReference;
        }

        /// <summary>
        /// Reference to the blob
        /// </summary>
        public string BlobReference => _blobReference;
    }
}
