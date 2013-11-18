using System;
using System.IO;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using CuttingEdge.Conditions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class AsynchronousBlockBlobRepository : IAsynchronousBlockBlobRepository
    {
        private readonly CloudBlobContainer _container;
        
        public AsynchronousBlockBlobRepository(string storageAccountConnectionString, string containerName)
        {
            Condition.Requires(storageAccountConnectionString).IsNotNullOrWhiteSpace();
            Condition.Requires(containerName).IsNotNullOrWhiteSpace();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            client.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(120), 3);
            _container = client.GetContainerReference(containerName);
        }

        public Task<IBlob> UploadAsync(string name, Stream stream)
        {
            return Task.Factory.StartNew<IBlob>(() =>
            {
                CloudBlockBlob blob = _container.GetBlockBlobReference(name);
                //blob.UploadFromStreamAsync(stream);
                blob.UploadFromStream(stream);
                return new BlockBlob(blob);
            });
        }

        public void Upload(string name, Stream stream)
        {
            Upload(name,stream, null, null);
        }

        public async void Upload(string name, Stream stream, Action<string> success, Action<string, Exception> failure)
        {
            try
            {
                CloudBlockBlob blob = _container.GetBlockBlobReference(name);
                await blob.UploadFromStreamAsync(stream);
                if (success != null) success(name);
            }
            catch (Exception ex)
            {
                if (failure != null) failure(name, ex);
            }
        }

        public IBlob Get(string name)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(name);
            return new BlockBlob(blob);
        }
    }
}
