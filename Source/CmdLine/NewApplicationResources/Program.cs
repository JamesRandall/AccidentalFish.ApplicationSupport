using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Azure.Configuration;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table;
using Nito.AsyncEx;

namespace NewApplicationResources
{
    /// <summary>
    /// Currently a straight and somewhat painful copy of the PS cmdlet. Am not refactoring as a upcoming version
    /// is moving to a pluggable configuration system to better support different resource types and providers
    /// </summary>
    class Program
    {
        private static bool _isVerbose;

        static void Main(string[] args)
        {
            Options options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                _isVerbose = options.Verbose;
                ConsoleColor oldColor = Console.ForegroundColor;
                try
                {
                    AsyncContext.Run(() => AsyncMain(options));
                }
                catch (ConsoleAppException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.ForegroundColor = oldColor;
                }
            }
        }

        private static async Task AsyncMain(Options options)
        {
            WriteVerbose($"Processing configuration file {options.Configuration}");
            if (!File.Exists(options.Configuration))
            {
                throw new ConsoleAppException("Configuration file does not exist");
            }

            IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(new UnityContainer());
            dependencyResolver.UseCore().UseAzure();

            bool useKeyVault = !string.IsNullOrWhiteSpace(options.KeyVaultClientId) && !string.IsNullOrWhiteSpace(options.KeyVaultClientKey) && !string.IsNullOrWhiteSpace(options.KeyVaultUri);
            IAsyncConfiguration keyVaultConfiguration = null;
            if (useKeyVault)
            {
                dependencyResolver.UseAsyncKeyVaultApplicationConfiguration(options.KeyVaultClientId, options.KeyVaultClientKey, options.KeyVaultUri);
                keyVaultConfiguration = dependencyResolver.Resolve<IAsyncKeyVaultConfiguration>();
                WriteVerbose($"Using key vault {options.KeyVaultUri}");
            }

            WriteVerbose("Reading settings");
            string[] settingsFiles = options.Settings.Split(',');
            ApplicationConfigurationSettings settings = ApplicationConfigurationSettings.FromFiles(settingsFiles);            
            WriteVerbose("Reading configuration");
            ApplicationConfiguration configuration = await ApplicationConfiguration.FromFileAsync(options.Configuration, settings,
                options.CheckForMissingSettings, keyVaultConfiguration, WriteVerbose);

            ApplyCorsRules(configuration);

            foreach (ApplicationComponent component in configuration.ApplicationComponents)
            {
                if (component.UsesServiceBus)
                {
                    WriteVerbose($"Creating service bus resources for component {component.Fqn}");
                    if (!String.IsNullOrWhiteSpace(component.DefaultTopicName))
                    {
                        NamespaceManager namespaceManager =
                            NamespaceManager.CreateFromConnectionString(component.ServiceBusConnectionString);

                        if (!namespaceManager.TopicExists(component.DefaultTopicName))
                        {
                            namespaceManager.CreateTopic(new TopicDescription(component.DefaultTopicName));
                        }

                        if (!String.IsNullOrWhiteSpace(component.DefaultSubscriptionName))
                        {
                            if (
                                !namespaceManager.SubscriptionExists(component.DefaultTopicName,
                                    component.DefaultSubscriptionName))
                            {
                                namespaceManager.CreateSubscription(
                                    new SubscriptionDescription(component.DefaultTopicName,
                                        component.DefaultSubscriptionName));
                            }
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(component.DefaultBrokeredMessageQueueName))
                    {
                        NamespaceManager namespaceManager =
                            NamespaceManager.CreateFromConnectionString(component.ServiceBusConnectionString);
                        if (!namespaceManager.QueueExists(component.DefaultBrokeredMessageQueueName))
                        {
                            namespaceManager.CreateQueue(component.DefaultBrokeredMessageQueueName);
                        }
                    }

                    foreach (ApplicationComponentSetting setting in component.Settings)
                    {
                        string resourceType = setting.ResourceType;
                        if (resourceType != null)
                        {
                            resourceType = resourceType.ToLower();
                            if (resourceType == "topic")
                            {
                                NamespaceManager namespaceManager =
                                    NamespaceManager.CreateFromConnectionString(component.ServiceBusConnectionString);
                                if (!namespaceManager.TopicExists(setting.Value))
                                {
                                    namespaceManager.CreateTopic(new TopicDescription(setting.Value));
                                }
                            }
                            else if (resourceType == "subscription")
                            {
                                NamespaceManager namespaceManager =
                                    NamespaceManager.CreateFromConnectionString(component.ServiceBusConnectionString);
                                string topicPath = setting.Attributes["topic"];
                                if (!namespaceManager.TopicExists(topicPath))
                                {
                                    namespaceManager.CreateTopic(new TopicDescription(topicPath));
                                }
                                if (!namespaceManager.SubscriptionExists(topicPath, setting.Value))
                                {
                                    namespaceManager.CreateSubscription(new SubscriptionDescription(topicPath,
                                        setting.Value));
                                }
                            }
                            else if (resourceType == "brokered-message-queue")
                            {
                                NamespaceManager namespaceManager =
                                    NamespaceManager.CreateFromConnectionString(component.ServiceBusConnectionString);
                                if (!namespaceManager.QueueExists(setting.Value))
                                {
                                    namespaceManager.CreateQueue(setting.Value);
                                }
                            }
                        }
                    }
                }

                if (component.UsesAzureStorage)
                {
                    CloudStorageAccount storageAccount =
                        CloudStorageAccount.Parse(component.StorageAccountConnectionString);
                    if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                    {
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer blobContainer =
                            blobClient.GetContainerReference(component.DefaultBlobContainerName);
                        blobContainer.CreateIfNotExists(
                            BlobContainerPublicAccessType(component.DefaultBlobContainerAccessType));

                        WriteVerbose($"Creating blob container {component.DefaultBlobContainerName} in {storageAccount.BlobEndpoint}");

                        if (component.Uploads != null)
                        {
                            foreach (string uploadFilename in component.Uploads)
                            {
                                string fullUploadFilename = Path.Combine(Path.GetDirectoryName(options.Configuration),
                                    uploadFilename);
                                CloudBlockBlob blob =
                                    blobContainer.GetBlockBlobReference(Path.GetFileName(uploadFilename));
                                blob.UploadFromFile(fullUploadFilename);
                                WriteVerbose($"Uploading file {uploadFilename} to blob container {component.DefaultBlobContainerName}");
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultLeaseBlockName))
                    {
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer blobContainer =
                            blobClient.GetContainerReference(component.DefaultLeaseBlockName);
                        blobContainer.CreateIfNotExists(
                            BlobContainerPublicAccessType(component.DefaultBlobContainerAccessType));

                        WriteVerbose($"Creating lease block container {component.DefaultLeaseBlockName} in {storageAccount.BlobEndpoint}");
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                    {
                        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                        CloudQueue queue = queueClient.GetQueueReference(component.DefaultQueueName);
                        queue.CreateIfNotExists();

                        WriteVerbose($"Creating queue {component.DefaultQueueName} in {storageAccount.QueueEndpoint}");
                    }

                    if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                    {
                        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                        CloudTable table = tableClient.GetTableReference(component.DefaultTableName);
                        table.CreateIfNotExists();

                        WriteVerbose($"Creating table {component.DefaultTableName} in {storageAccount.TableEndpoint}");

                        if (!string.IsNullOrWhiteSpace(component.TableData))
                        {
                            XDocument document;
                            string tableDataFilename = Path.Combine(Path.GetDirectoryName(options.Configuration),
                                component.TableData);
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
                                WriteVerbose($"Unable to load table data document {tableDataFilename}. Error: {ex.Message}");
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

                                WriteVerbose($"Creating table {setting.Value} in {storageAccount.TableEndpoint}");
                            }
                            else if (resourceType == "queue")
                            {
                                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                                CloudQueue queue = queueClient.GetQueueReference(setting.Value);
                                queue.CreateIfNotExists();
                                WriteVerbose($"Creating queue {setting.Value} in {storageAccount.TableEndpoint}");
                            }
                            else if (resourceType == "blob-container")
                            {
                                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                                CloudBlobContainer blobContainer = blobClient.GetContainerReference(setting.Value);
                                blobContainer.CreateIfNotExists();
                                WriteVerbose($"Creating blob container {setting.Value} in {storageAccount.TableEndpoint}");
                            }
                        }
                    }
                }
            }
        }

        private static void WriteVerbose(string message)
        {
            if (_isVerbose)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
            }
        }

        private static BlobContainerPublicAccessType BlobContainerPublicAccessType(
            BlobContainerPublicAccessTypeEnum value)
        {
            switch (value)
            {
                case BlobContainerPublicAccessTypeEnum.Off:
                    return Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Off;

                case BlobContainerPublicAccessTypeEnum.Blob:
                    return Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Blob;

                case BlobContainerPublicAccessTypeEnum.Container:
                    return Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Container;
            }
            throw new InvalidOperationException("Invalid blob access type");
        }

        private static void UploadTableData(CloudTable table, XDocument document)
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

        private static EntityProperty GetEntityProperty(XElement xProperty)
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

        private static  EntityProperty GetEntityPropertyByInference(string value)
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

        private static EntityProperty GetEntityPropertyForType(string type, string value)
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
            throw new InvalidOperationException($"Type {type} not understood");
        }

        private static void ApplyCorsRules(ApplicationConfiguration configuration)
        {
            foreach (ApplicationStorageAccount storageAccount in configuration.StorageAccounts.Values)
            {
                CloudStorageAccount cloudStorageAccount = null;
                CloudTableClient tableClient = null;
                ServiceProperties tableProperties = null;
                CloudBlobClient blobClient = null;
                ServiceProperties blobProperties = null;
                CloudQueueClient queueClient = null;
                ServiceProperties queueProperties = null;
                foreach (ApplicationCorsRule rule in storageAccount.CorsRules)
                {
                    if (cloudStorageAccount == null)
                    {
                        cloudStorageAccount = CloudStorageAccount.Parse(storageAccount.ConnectionString);
                    }

                    if (rule.ApplyToTables)
                    {
                        if (tableClient == null)
                        {
                            tableClient = cloudStorageAccount.CreateCloudTableClient();
                            tableProperties = tableClient.GetServiceProperties();
                        }

                        tableProperties.Cors.CorsRules.Add(CreateCorsRule(rule));
                    }

                    if (rule.ApplyToQueues)
                    {
                        if (queueClient == null)
                        {
                            queueClient = cloudStorageAccount.CreateCloudQueueClient();
                            queueProperties = queueClient.GetServiceProperties();
                        }
                        queueProperties.Cors.CorsRules.Add(CreateCorsRule(rule));
                    }

                    if (rule.ApplyToBlobs)
                    {
                        if (blobClient == null)
                        {
                            blobClient = cloudStorageAccount.CreateCloudBlobClient();
                            blobProperties = blobClient.GetServiceProperties();
                        }
                        blobProperties.Cors.CorsRules.Add(CreateCorsRule(rule));
                    }
                }

                tableClient?.SetServiceProperties(tableProperties);
                blobClient?.SetServiceProperties(blobProperties);
                queueClient?.SetServiceProperties(queueProperties);
            }
        }

        private static CorsRule CreateCorsRule(ApplicationCorsRule rule)
        {
            return new CorsRule
            {
                AllowedHeaders = new List<string>(rule.AllowedHeaders.Split(',')),
                AllowedMethods = GetCorsMethods(rule.AllowedVerbs),
                AllowedOrigins = new List<string>(rule.AllowedOrigins.Split(',')),
                ExposedHeaders = new List<string>(rule.ExposedHeaders.Split(',')),
                MaxAgeInSeconds = rule.MaxAgeSeconds
            };
        }

        private static CorsHttpMethods GetCorsMethods(string verbString)
        {
            Dictionary<string, CorsHttpMethods> lookup = new Dictionary<string, CorsHttpMethods>
            {
                {"PUT", CorsHttpMethods.Put},
                {"POST", CorsHttpMethods.Post},
                {"GET", CorsHttpMethods.Get},
                {"DELETE", CorsHttpMethods.Delete},
                {"HEAD", CorsHttpMethods.Head},
                {"OPTIONS", CorsHttpMethods.Options},
                {"TRACE", CorsHttpMethods.Trace},
                {"MERGE", CorsHttpMethods.Merge},
                {"CONNECT", CorsHttpMethods.Connect}
            };

            CorsHttpMethods result = CorsHttpMethods.None;
            string[] verbs = verbString.Split(',');
            foreach (string verb in verbs)
            {
                result |= lookup[verb];
            }
            return result;
        }
    }
}
