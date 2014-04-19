using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class BlobMultipartStreamProvider : MultipartStreamProvider
    {
        private readonly CloudBlobContainer _blobContainer;

        public BlobMultipartStreamProvider(CloudBlobContainer blobContainer)
        {
            _blobContainer = blobContainer;
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            Stream stream = null;
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                if (!String.IsNullOrWhiteSpace(contentDisposition.FileName))
                {
                    CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(contentDisposition.FileName);
                    stream = blob.OpenWrite();
                }
            }
            return stream;
        }
    }
}
