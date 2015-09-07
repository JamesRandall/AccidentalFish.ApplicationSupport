using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class BlobMultipartStreamProvider : MultipartStreamProvider
    {
        private readonly Func<ContentDispositionHeaderValue, string> _getBlobName;
        private readonly CloudBlobContainer _blobContainer;

        public BlobMultipartStreamProvider(CloudBlobContainer blobContainer)
        {
            _getBlobName = null;
            _blobContainer = blobContainer;
        }

        public BlobMultipartStreamProvider(CloudBlobContainer blobContainer,
            Func<ContentDispositionHeaderValue, string> getBlobName) : this(blobContainer)
        {
            _getBlobName = getBlobName;
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            Stream stream = null;
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                string blobFilename = null;
                if (_getBlobName != null)
                {
                    blobFilename = _getBlobName(contentDisposition);
                }
                else if (!String.IsNullOrWhiteSpace(contentDisposition.FileName))
                {
                    blobFilename = contentDisposition.FileName;
                }

                if (!String.IsNullOrWhiteSpace(blobFilename))
                {
                    CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(blobFilename);
                    stream = blob.OpenWrite();
                }
            }
            return stream;
        }
    }
}
