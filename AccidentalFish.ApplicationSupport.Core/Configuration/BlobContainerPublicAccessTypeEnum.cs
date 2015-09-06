namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Public blob container access
    /// </summary>
    public enum BlobContainerPublicAccessTypeEnum
    {
        /// <summary>
        /// No public access
        /// </summary>
        Off,
        /// <summary>
        /// Access to blobs who's name is known
        /// </summary>
        Blob,
        /// <summary>
        /// Access to the whole container
        /// </summary>
        Container
    }
}
