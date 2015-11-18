using System;
using System.IO;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Blobs
{
    /// <summary>
    /// Class for storing and accessing blob files
    /// </summary>
    public interface IAsynchronousBlockBlobRepository
    {
        /// <summary>
        /// Uploads a blob to the store giving it the specified name
        /// </summary>
        /// <param name="name">The name of the blob</param>
        /// <param name="stream">The source stream for the blob data</param>
        /// <returns>The blob reference</returns>
        Task<IBlob> UploadAsync(string name, Stream stream);
        /// <summary>
        /// Get a blob reference
        /// </summary>
        /// <param name="name">The name of the blob to download</param>
        /// <returns>A blob implementation</returns>
        IBlob Get(string name);
        /// <summary>
        /// The endpoint of the blob container
        /// </summary>
        string Endpoint { get; }
    }
}
