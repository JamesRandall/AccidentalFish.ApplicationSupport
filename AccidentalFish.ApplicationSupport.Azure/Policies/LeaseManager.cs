using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccidentalFish.ApplicationSupport.Azure.NoSql
{
    internal class LeaseManager<T> : ILeaseManager<T>
    {
        private readonly CloudBlobContainer _container;

        public LeaseManager(
            string storageAccountConnectionString,
            string leaseBlockName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            _container = client.GetContainerReference(leaseBlockName);
        }

        public async Task<bool> CreateLeaseObjectIfNotExist(T key)
        {
            string leaseName = GetLeaseName(key);
            CloudBlockBlob blob = _container.GetBlockBlobReference(leaseName);
            if (!(await blob.ExistsAsync()))
            {
                await blob.UploadTextAsync("");
                return true;
            }
            return false;
        }

        public async Task<string> Lease(T key)
        {
            return await Lease(key, TimeSpan.FromSeconds(30));
        }

        public async Task<string> Lease(T key, TimeSpan leaseTime)
        {
            string leaseName = GetLeaseName(key);
            CloudBlockBlob blob = _container.GetBlockBlobReference(leaseName);
            try
            {
                string leaseId = await blob.AcquireLeaseAsync(leaseTime, Guid.NewGuid().ToString());
                return leaseId;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 400)
                {
                    throw new UnableToAcquireLease("Unable to acquire lease", ex);
                }
                throw;
            }
            
        }

        public async Task Release(T key, string leaseId)
        {
            string leaseName = GetLeaseName(key);
            CloudBlockBlob blob = _container.GetBlockBlobReference(leaseName);
            await blob.ReleaseLeaseAsync(new AccessCondition
            {
                LeaseId = leaseId
            });
        }

        public async Task Renew(T key, string leaseId)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(GetLeaseName(key));
            await blob.RenewLeaseAsync(new AccessCondition
            {
                LeaseId = leaseId
            });
        }

        private static string GetLeaseName(T key)
        {
            string leaseName = String.Format("{0}.lck", key);
            return leaseName;
        }
    }
}
