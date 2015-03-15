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
            Uploads = new List<string>();
        }

        public string Fqn { get; set; }

        public string SqlServerConnectionString { get; set; }

        public string StorageAccountConnectionString { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string DbContextType { get; set; }

        public BlobContainerPublicAccessType DefaultBlobContainerAccessType { get; set; }

        public string DefaultBlobContainerName { get; set; }

        public string DefaultQueueName { get; set; }

        public string DefaultTableName { get; set; }

        public string DefaultLeaseBlockName { get; set; }

        public string DefaultTopicName { get; set; }

        public string DefaultSubscriptionName { get; set; }

        public string TableData { get; set; }

        public List<ApplicationComponentSetting> Settings { get; set; }

        public List<string> Uploads { get; set; } 

        public bool UsesAzureStorage
        {
            get
            {
                return !String.IsNullOrWhiteSpace(StorageAccountConnectionString);
            }
        }

        public bool UsesServiceBus
        {
            get { return !String.IsNullOrWhiteSpace(ServiceBusConnectionString); }
        }
    }
}
