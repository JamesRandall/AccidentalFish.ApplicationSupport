using System;
using System.IO;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using static System.String;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class AsynchronousBlockBlobRepository : IAsynchronousBlockBlobRepository
    {
        private readonly CloudBlobContainer _container;
        private readonly string _endpoint;
        
        public AsynchronousBlockBlobRepository(string storageAccountConnectionString, string containerName)
        {
            if (IsNullOrWhiteSpace(containerName)) throw new ArgumentNullException(nameof(containerName));
            if (IsNullOrWhiteSpace(storageAccountConnectionString)) throw new ArgumentNullException(nameof(storageAccountConnectionString));

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            client.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(120), 3);
            _container = client.GetContainerReference(containerName);

            _endpoint = $"{client.BaseUri}{containerName}";
        }

        internal CloudBlobContainer CloudBlobContainer => _container;

        public Task<IBlob> UploadAsync(string name, Stream stream)
        {
            return Task.Run<IBlob>(() =>
            {
                CloudBlockBlob blob = _container.GetBlockBlobReference(name);
                blob.UploadFromStream(stream);
                return new BlockBlob(blob);
            });
        }

        public IBlob Get(string name)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(name);
            return new BlockBlob(blob);
        }

        public string Endpoint => _endpoint;

        internal CloudBlobContainer Container => _container;
    }
}
