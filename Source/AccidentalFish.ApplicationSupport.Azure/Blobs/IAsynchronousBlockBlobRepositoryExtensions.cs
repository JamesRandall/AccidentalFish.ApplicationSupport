using System;
using System.Net.Http;
using System.Net.Http.Headers;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    // ReSharper disable once InconsistentNaming
    public static class IAsynchronousBlockBlobRepositoryExtensions
    {
        public static MultipartStreamProvider GetMultipartStreamProvider(this IAsynchronousBlockBlobRepository repository)
        {
            AsynchronousBlockBlobRepository blobRepository = (AsynchronousBlockBlobRepository) repository;
            return new BlobMultipartStreamProvider(blobRepository.CloudBlobContainer);
        }

        public static MultipartStreamProvider GetMultipartStreamProvider(this IAsynchronousBlockBlobRepository repository, Func<ContentDispositionHeaderValue, string> getBlobName)
        {
            AsynchronousBlockBlobRepository blobRepository = (AsynchronousBlockBlobRepository)repository;
            return new BlobMultipartStreamProvider(blobRepository.CloudBlobContainer, getBlobName);
        }

        public static string GetSharedAccessSignature(this IAsynchronousBlockBlobRepository repository, SharedAccessBlobPolicy policy)
        {
            AsynchronousBlockBlobRepository blobRepository = (AsynchronousBlockBlobRepository)repository;
            return blobRepository.Container.GetSharedAccessSignature(policy);
        }

        public static CloudBlobContainer Container(this IAsynchronousBlockBlobRepository repository)
        {
            AsynchronousBlockBlobRepository blobRepository = (AsynchronousBlockBlobRepository)repository;
            return blobRepository.Container;
        }
    }
}
