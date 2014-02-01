using System;
using System.IO;
using System.Management.Automation;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Powershell
{
    [Cmdlet(VerbsCommon.New, "ApplicationResources")]
    public class NewApplicationResources : PSCmdlet
    {
        [Parameter(HelpMessage = "The application configuration file", Mandatory = true)]
        public string Configuration { get; set; }

        [Parameter(HelpMessage = "Optional settings file", Mandatory=false)]
        public string Settings { get; set; }

        protected override void ProcessRecord()
        {
            if (!File.Exists(Configuration))
            {
                throw new InvalidOperationException("Configuration file does not exist");
            }

            ApplicationConfigurationSettings settings = String.IsNullOrWhiteSpace(Settings) ? null : ApplicationConfigurationSettings.FromFile(Settings);
            ApplicationConfiguration configuration = ApplicationConfiguration.FromFile(Configuration, settings);

            foreach (ApplicationComponent component in configuration.ApplicationComponents)
            {
                if (component.UsesAzureStorage)
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(component.StorageAccountConnectionString);
                    if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                    {
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer blobContainer = blobClient.GetContainerReference(component.DefaultBlobContainerName);
                        blobContainer.CreateIfNotExists(component.DefaultBlobContainerAccessType);

                        WriteVerbose(String.Format("Creating blob container {0} in {1}", component.DefaultBlobContainerName, storageAccount.BlobEndpoint));
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultLeaseBlockName))
                    {
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer blobContainer = blobClient.GetContainerReference(component.DefaultLeaseBlockName);
                        blobContainer.CreateIfNotExists(component.DefaultBlobContainerAccessType);

                        WriteVerbose(String.Format("Creating lease block container {0} in {1}", component.DefaultLeaseBlockName, storageAccount.BlobEndpoint));
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                    {
                        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                        CloudQueue queue = queueClient.GetQueueReference(component.DefaultQueueName);
                        queue.CreateIfNotExists();

                        WriteVerbose(String.Format("Creating queue {0} in {1}", component.DefaultQueueName, storageAccount.QueueEndpoint));
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                    {
                        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                        CloudTable table = tableClient.GetTableReference(component.DefaultTableName);
                        table.CreateIfNotExists();

                        WriteVerbose(String.Format("Creating table {0} in {1}", component.DefaultTableName, storageAccount.TableEndpoint));
                    }

                    foreach (ApplicationComponentSetting setting in component.Settings)
                    {
                        string resourceType = setting.ResourceType;
                        if (resourceType != null)
                        {
                            resourceType = resourceType.ToLower();
                            if (resourceType == "table")
                            {
                                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                                CloudTable table = tableClient.GetTableReference(setting.Value);
                                table.CreateIfNotExists();
                            }
                            else if (resourceType == "queue")
                            {
                                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                                CloudQueue queue = queueClient.GetQueueReference(setting.Value);
                                queue.CreateIfNotExists();
                            }
                            else if (resourceType == "blob-container")
                            {
                                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                                CloudBlobContainer blobContainer = blobClient.GetContainerReference(setting.Value);
                                blobContainer.CreateIfNotExists();
                            }
                        }
                    }
                }
            }
        }

        public void CheatRun()
        {
            ProcessRecord();
        }
    }
}
