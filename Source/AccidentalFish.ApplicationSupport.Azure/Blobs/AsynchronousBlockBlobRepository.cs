using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using static System.String;

namespace AccidentalFish.ApplicationSupport.Azure.Blobs
{
    internal class AsynchronousBlockBlobRepository : IAsynchronousBlockBlobRepository
    {
        private readonly IAzureAssemblyLogger _logger;
        private readonly CloudBlobContainer _container;
        private readonly string _endpoint;
        
        public AsynchronousBlockBlobRepository(
            string storageAccountConnectionString,
            string containerName,
            IAzureAssemblyLogger logger)
        {
            _logger = logger;
            if (IsNullOrWhiteSpace(containerName)) throw new ArgumentNullException(nameof(containerName));
            if (IsNullOrWhiteSpace(storageAccountConnectionString)) throw new ArgumentNullException(nameof(storageAccountConnectionString));

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            client.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(120), 3);
            _container = client.GetContainerReference(containerName);

            // Development storage doesn't return a '/' on the end, live storage does. 
            _endpoint = !client.BaseUri.ToString().EndsWith("/") ? $"{client.BaseUri}/{containerName}" : $"{client.BaseUri}{containerName}";
            

            _logger?.Verbose("AsynchronousBlockBlobRepository: create repository for endpoint {0}", _endpoint);
        }

        internal CloudBlobContainer CloudBlobContainer => _container;

        public Task<IBlob> UploadAsync(string name, Stream stream)
        {
            _logger?.Verbose("AsynchronousBlockBlobRepository: UploadAsync - attempting to upload blob {0}", name);
            return Task.Run<IBlob>(() =>
            {
                CloudBlockBlob blob = _container.GetBlockBlobReference(name);
                blob.UploadFromStream(stream);
                
                BlockBlob result = new BlockBlob(blob, name, _logger);

                _logger?.Verbose("AsynchronousBlockBlobRepository: UploadAsync - successfull uploaded blob {0}", name);

                return result;
            });
        }

        public IBlob Get(string name)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(name);
            return new BlockBlob(blob, name, _logger);
        }

        public async Task<IReadOnlyCollection<IBlob>> ListAsync()
        {
            BlobContinuationToken continuationToken = null;
            List<IBlob> results = new List<IBlob>();
            do
            {
                var response = await _container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results.Select(x => new BlockBlob(x as CloudBlockBlob, Path.GetFileName(x.Uri.LocalPath), _logger)));
            }
            while (continuationToken != null);
            return results;
        }

        public async Task<IReadOnlyCollection<IBlob>> ListAsync(string prefix)
        {
            BlobContinuationToken continuationToken = null;
            List<IBlob> results = new List<IBlob>();
            do
            {
                var response = await _container.ListBlobsSegmentedAsync(prefix, continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results.Select(x => new BlockBlob(x as CloudBlockBlob, Path.GetFileName(x.Uri.LocalPath), _logger)));
            }
            while (continuationToken != null);
            return results;
        }

        public string Endpoint => _endpoint;

        internal CloudBlobContainer Container => _container;
    }
}
