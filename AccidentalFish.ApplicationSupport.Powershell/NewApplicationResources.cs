using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Xml.Linq;
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
            WriteVerbose(String.Format("Processing configuration file {0}", Configuration));
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

                        if (component.Uploads != null)
                        {
                            foreach (string uploadFilename in component.Uploads)
                            {
                                string fullUploadFilename = Path.Combine(Path.GetDirectoryName(Configuration), uploadFilename);
                                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(Path.GetFileName(uploadFilename));
                                blob.UploadFromFile(fullUploadFilename, FileMode.Open);
                                WriteVerbose(String.Format("Uploading file {0} to blob container {1}", uploadFilename, component.DefaultBlobContainerName));
                            }
                        }
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

                        if (!string.IsNullOrWhiteSpace(component.TableData))
                        {
                            XDocument document = null;
                            string tableDataFilename = Path.Combine(Path.GetDirectoryName(Configuration), component.TableData);
                            try
                            {

                                using (StreamReader reader = new StreamReader(tableDataFilename))
                                {
                                    document = XDocument.Load(reader);
                                }
                            }
                            catch (Exception ex)
                            {
                                document = null;
                                WriteVerbose(String.Format("Unable to load table data document {0}. Error: {1}", tableDataFilename, ex.Message));
                            }
                            if (document != null)
                            {
                                UploadTableData(table, document);
                            }   
                        }
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

                                WriteVerbose(String.Format("Creating table {0} in {1}", setting.Value, storageAccount.TableEndpoint));
                            }
                            else if (resourceType == "queue")
                            {
                                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                                CloudQueue queue = queueClient.GetQueueReference(setting.Value);
                                queue.CreateIfNotExists();
                                WriteVerbose(String.Format("Creating queue {0} in {1}", setting.Value, storageAccount.TableEndpoint));
                            }
                            else if (resourceType == "blob-container")
                            {
                                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                                CloudBlobContainer blobContainer = blobClient.GetContainerReference(setting.Value);
                                blobContainer.CreateIfNotExists();
                                WriteVerbose(String.Format("Creating blob container {0} in {1}", setting.Value, storageAccount.TableEndpoint));
                            }
                        }
                    }
                }
            }
        }

        private void UploadTableData(CloudTable table, XDocument document)
        {
            foreach (XElement xEntity in document.Root.Elements("entity"))
            {
                string partitionKey = xEntity.Element("PartitionKey").Value;
                XElement xRowKey = xEntity.Element("RowKey");
                string rowKey = xRowKey != null ? xRowKey.Value : "";
                DynamicTableEntity tableEntity = new DynamicTableEntity(partitionKey, rowKey);
                foreach (XElement xProperty in xEntity.Elements())
                {
                    if (xProperty.Name != "PartitionKey" && xProperty.Name != "RowKey")
                    {
                        EntityProperty property = GetEntityProperty(xProperty);
                        tableEntity[xProperty.Name.LocalName] = property;
                    }
                }
                table.Execute(TableOperation.InsertOrReplace(tableEntity));
            }
        }

        private EntityProperty GetEntityProperty(XElement xProperty)
        {
            XAttribute type = xProperty.Attribute("type");
            if (type != null)
            {
                return GetEntityPropertyForType(type.Value, xProperty.Value);
            }
            else
            {
                return GetEntityPropertyByInference(xProperty.Value);
            }
        }

        private EntityProperty GetEntityPropertyByInference(string value)
        {
            DateTime dateTimeValue;
            DateTimeOffset dateTimeOffsetValue;
            Guid guidValue;
            double doubleValue;
            bool boolValue;

            if (Guid.TryParse(value, out guidValue))
            {
                return new EntityProperty(guidValue);
            }
            if (DateTimeOffset.TryParse(value, out dateTimeOffsetValue))
            {
                return new EntityProperty(dateTimeOffsetValue);
            }
            if (DateTime.TryParse(value, out dateTimeValue))
            {
                return new EntityProperty(dateTimeValue);
            }
            if (bool.TryParse(value, out boolValue))
            {
                return new EntityProperty(boolValue);
            }
            if (double.TryParse(value, out doubleValue))
            {
                return new EntityProperty(doubleValue);
            }
            return new EntityProperty(value);
        }

        private EntityProperty GetEntityPropertyForType(string type, string value)
        {
            if (type == "datetime")
            {
                DateTime? typedValue = string.IsNullOrWhiteSpace(value) ? null : new DateTime?(DateTime.Parse(value));
                return new EntityProperty(typedValue);
            }
            if (type == "datetimeoffset")
            {
                DateTimeOffset? typedValue = string.IsNullOrWhiteSpace(value)
                    ? null
                    : new DateTimeOffset?(DateTimeOffset.Parse(value));
                return new EntityProperty(typedValue);
            }
            if (type == "guid")
            {
                Guid? typedValue = string.IsNullOrWhiteSpace(value) ? null : new Guid?(Guid.Parse(value));
                return new EntityProperty(typedValue);
            }
            if (type == "int")
            {
                int? typedValue = string.IsNullOrWhiteSpace(value) ? null : new int?(int.Parse(value));
                return new EntityProperty(typedValue);
            }
            if (type == "double")
            {
                double? typedValue = string.IsNullOrWhiteSpace(value) ? null : new double?(double.Parse(value));
                return new EntityProperty(typedValue);
            }
            if (type == "long")
            {
                long? typedValue = string.IsNullOrWhiteSpace(value) ? null : new long?(long.Parse(value));
                return new EntityProperty(typedValue);
            }
            if (type == "bool")
            {
                bool? typedValue = string.IsNullOrWhiteSpace(value) ? null : new bool?(bool.Parse(value));
                return new EntityProperty(typedValue);
            }
            if (type == "string")
            {
                return new EntityProperty(value);
            }
            throw new InvalidOperationException(String.Format("Type {0} not understood", type));
        }

        public void CheatRun()
        {
            ProcessRecord();
        }
    }
}
