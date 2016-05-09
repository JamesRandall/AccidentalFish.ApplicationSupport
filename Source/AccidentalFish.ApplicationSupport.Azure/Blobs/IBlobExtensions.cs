using AccidentalFish.ApplicationSupport.Core.Blobs;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    // ReSharper disable once InconsistentNaming
    public static class IBlobExtensions
    {
        public static string GetSharedAccessSignature(this IBlob blob, SharedAccessBlobPolicy policy)
        {
            BlockBlob azureBlob = (BlockBlob) blob;
            return azureBlob.CloudBlockBlob.GetSharedAccessSignature(policy);
        }

        public static CloudBlockBlob CloudBlockBlob(this IBlob blob)
        {
            BlockBlob azureBlob = (BlockBlob) blob;
            return azureBlob.CloudBlockBlob;
        }
    }
}
