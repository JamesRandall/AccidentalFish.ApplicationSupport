using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Powershell
{
    [Cmdlet(VerbsCommon.Remove, "ApplicationResources")]
    public class RemoveApplicationResources : AsyncPSCmdlet
    {
        [Parameter(HelpMessage = "The application configuration file", Mandatory = true)]
        public string Configuration { get; set; }

        [Parameter(HelpMessage = "Optional settings file", Mandatory = false)]
        public string[] Settings { get; set; }

        [Parameter(HelpMessage = "Defaults to false, if set to true then an exception will be thrown if a setting is required in a configuration file but not supplied", Mandatory = false)]
        public bool CheckForMissingSettings { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            WriteVerbose(String.Format("Processing configuration file {0}", Configuration));
            if (!File.Exists(Configuration))
            {
                throw new InvalidOperationException("Configuration file does not exist");
            }

            ApplicationConfigurationSettings settings = Settings != null && Settings.Length > 0 ? ApplicationConfigurationSettings.FromFiles(Settings) : null;
            ApplicationConfiguration configuration = await ApplicationConfiguration.FromFileAsync(Configuration, settings, CheckForMissingSettings);

            foreach (ApplicationComponent component in configuration.ApplicationComponents)
            {
                if (component.UsesAzureStorage)
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(component.StorageAccountConnectionString);
                    if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                    {
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer blobContainer = blobClient.GetContainerReference(component.DefaultBlobContainerName);
                        blobContainer.DeleteIfExists();

                        WriteVerbose(String.Format("Deleting blob container {0} in {1}", component.DefaultBlobContainerName, storageAccount.BlobEndpoint));
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultLeaseBlockName))
                    {
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer blobContainer = blobClient.GetContainerReference(component.DefaultLeaseBlockName);
                        blobContainer.DeleteIfExists();

                        WriteVerbose(String.Format("Deleting lease block container {0} in {1}", component.DefaultLeaseBlockName, storageAccount.BlobEndpoint));
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                    {
                        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                        CloudQueue queue = queueClient.GetQueueReference(component.DefaultQueueName);
                        queue.DeleteIfExists();

                        WriteVerbose(String.Format("Deleting queue {0} in {1}", component.DefaultQueueName, storageAccount.QueueEndpoint));
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                    {
                        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                        CloudTable table = tableClient.GetTableReference(component.DefaultTableName);
                        table.DeleteIfExists();

                        WriteVerbose(String.Format("Deleting table {0} in {1}", component.DefaultTableName, storageAccount.TableEndpoint));
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
                                table.DeleteIfExists();

                                WriteVerbose(String.Format("Deleting table {0} in {1}", setting.Value, storageAccount.TableEndpoint));
                            }
                            else if (resourceType == "queue")
                            {
                                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                                CloudQueue queue = queueClient.GetQueueReference(setting.Value);
                                queue.DeleteIfExists();
                                WriteVerbose(String.Format("Deleting queue {0} in {1}", setting.Value, storageAccount.TableEndpoint));
                            }
                            else if (resourceType == "blob-container")
                            {
                                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                                CloudBlobContainer blobContainer = blobClient.GetContainerReference(setting.Value);
                                blobContainer.DeleteIfExists();
                                WriteVerbose(String.Format("Creating blob container {0} in {1}", setting.Value, storageAccount.TableEndpoint));
                            }
                        }
                    }
                }
            }
        }
    }
}
