using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    public class ApplicationComponent
    {
        public ApplicationComponent()
        {
            Settings = new List<ApplicationComponentSetting>();
        }

        public string Fqn { get; set; }

        public string SqlServerConnectionString { get; set; }

        public string StorageAccountConnectionString { get; set; }

        public string DbContextType { get; set; }

        public BlobContainerPublicAccessType DefaultBlobContainerAccessType { get; set; }

        public string DefaultBlobContainerName { get; set; }

        public string DefaultQueueName { get; set; }

        public string DefaultTableName { get; set; }

        public string DefaultLeaseBlockName { get; set; }

        public List<ApplicationComponentSetting> Settings { get; set; }

        public bool UsesAzureStorage
        {
            get
            {
                return !String.IsNullOrWhiteSpace(StorageAccountConnectionString);
            }
        }
    }
}
