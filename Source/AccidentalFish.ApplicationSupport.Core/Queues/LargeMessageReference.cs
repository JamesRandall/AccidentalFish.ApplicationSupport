namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Used by the ILargeMessageQueueFactory and underlying queue to provide a reference to a blob containing the
    /// large message
    /// </summary>
    public class LargeMessageReference
    {
        /// <summary>
        /// Blob name
        /// </summary>
        public string BlobReference { get; set; }
    }
}
