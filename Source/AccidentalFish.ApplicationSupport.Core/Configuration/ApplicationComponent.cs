using System;
using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Represents the configuration for an application component in a configuration file
    /// </summary>
    public class ApplicationComponent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationComponent()
        {
            Settings = new List<ApplicationComponentSetting>();
            Uploads = new List<string>();
        }

        /// <summary>
        /// Fully qualified name
        /// </summary>
        public string Fqn { get; set; }

        /// <summary>
        /// Sql Server connection string
        /// </summary>
        public string SqlServerConnectionString { get; set; }

        /// <summary>
        /// Storage account connection string
        /// </summary>
        public string StorageAccountConnectionString { get; set; }

        /// <summary>
        /// Service bus connection string
        /// </summary>
        public string ServiceBusConnectionString { get; set; }

        /// <summary>
        /// SQL database context type
        /// </summary>
        public string DbContextType { get; set; }

        /// <summary>
        /// Default blob container access type
        /// </summary>
        public BlobContainerPublicAccessTypeEnum DefaultBlobContainerAccessType { get; set; }

        /// <summary>
        /// Default blob container name
        /// </summary>
        public string DefaultBlobContainerName { get; set; }

        /// <summary>
        /// Default queue name
        /// </summary>
        public string DefaultQueueName { get; set; }

        /// <summary>
        /// Default table name
        /// </summary>
        public string DefaultTableName { get; set; }

        /// <summary>
        /// Default lease block name
        /// </summary>
        public string DefaultLeaseBlockName { get; set; }

        /// <summary>
        /// Default topic name
        /// </summary>
        public string DefaultTopicName { get; set; }

        /// <summary>
        /// Default subscription name
        /// </summary>
        public string DefaultSubscriptionName { get; set; }

        /// <summary>
        /// Table data upload file
        /// </summary>
        public string TableData { get; set; }

        /// <summary>
        /// Component application settings
        /// </summary>
        public List<ApplicationComponentSetting> Settings { get; set; }

        /// <summary>
        /// Blob upload filenames
        /// </summary>
        public List<string> Uploads { get; set; } 

        /// <summary>
        /// Does the component use Azure storage
        /// </summary>
        public bool UsesAzureStorage
        {
            get
            {
                return !String.IsNullOrWhiteSpace(StorageAccountConnectionString);
            }
        }

        /// <summary>
        /// Does the component use the service bus
        /// </summary>
        public bool UsesServiceBus
        {
            get { return !String.IsNullOrWhiteSpace(ServiceBusConnectionString); }
        }

        /// <summary>
        /// Default brokered message queue name
        /// </summary>
        public string DefaultBrokeredMessageQueueName { get; set; }
    }
}
