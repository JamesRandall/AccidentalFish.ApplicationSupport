using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Describes an applications configuration
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ApplicationConfiguration()
        {
            SqlServerConnectionStrings = new Dictionary<string, string>();
            StorageAccounts = new Dictionary<string, ApplicationStorageAccount>();
            ServiceBusConnectionStrings = new Dictionary<string, string>();
            ApplicationComponents = new List<ApplicationComponent>();
        }

        /// <summary>
        /// SQL Server connection strings in use
        /// </summary>
        public Dictionary<string, string> SqlServerConnectionStrings { get; set; }

        /// <summary>
        /// Storage accounts in use
        /// </summary>
        public Dictionary<string, ApplicationStorageAccount> StorageAccounts { get; set; }

        /// <summary>
        /// Service bus connection strings in use
        /// </summary>
        public Dictionary<string, string> ServiceBusConnectionStrings { get; set; } 

        /// <summary>
        /// Application components defined
        /// </summary>
        public List<ApplicationComponent> ApplicationComponents { get; set; }

        /// <summary>
        /// Loads the application configuration from an XML file
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <param name="settings">An optional settings file</param>
        /// <param name="checkForMissingSettings">If set to true then any missing settings generate an exception</param>
        /// <returns></returns>
        public static ApplicationConfiguration FromFile(string filename, ApplicationConfigurationSettings settings, bool checkForMissingSettings)
        {
            ApplicationConfiguration configuration = new ApplicationConfiguration();
            XDocument document;
            using (StreamReader reader = new StreamReader(filename))
            {
                if (settings != null)
                {
                    string processedXml = settings.Merge(reader);

                    if (checkForMissingSettings)
                    {
                        Regex settingPattern = new Regex(@"(?<!\{)\{\{(?!\{).*(?<!\})\}\}(?!\})");
                        if (settingPattern.Match(processedXml).Success)
                        {
                            throw new MissingSettingException();
                        }
                    }

                    document = XDocument.Parse(processedXml);
                }
                else
                {
                    document = XDocument.Load(reader);
                }
            }

            if (document.Root == null) return null;
            
            document.Root.XPathSelectElements("infrastructure/sql-server").ToList().ForEach(element =>
            {
                var xfqn = element.Element("fqn");
                if (xfqn != null)
                {
                    var xconnectionstring = element.Element("connection-string");
                    if (xconnectionstring != null)
                        configuration.SqlServerConnectionStrings.Add(xfqn.Value, xconnectionstring.Value);
                }
            });
            document.Root.XPathSelectElements("infrastructure/storage-account").ToList().ForEach(element =>
            {
                ApplicationStorageAccount storageAccount = new ApplicationStorageAccount(element);
                configuration.StorageAccounts.Add(storageAccount.Fqn, storageAccount);
            });
            document.Root.XPathSelectElements("infrastructure/service-bus").ToList().ForEach(element =>
            {
                var xfqn = element.Element("fqn");
                if (xfqn != null)
                {
                    var xconnectionstring = element.Element("connection-string");
                    if (xconnectionstring != null)
                        configuration.ServiceBusConnectionStrings.Add(xfqn.Value, xconnectionstring.Value);
                }
            });

            document.Root.Elements("component").ToList().ForEach(element =>
            {
                ApplicationComponent component = new ApplicationComponent
                {
                    Fqn = element.Attribute("fqn").Value
                };
                XElement sqlServerElement = element.Element("sql-server");
                XElement storageElement = element.Element("storage-account");
                XElement serviceBusElement = element.Element("service-bus");
                XElement dbContextTypeElement = element.Element("db-context-type");
                XElement defaultBlobContainerNameElement = element.Element("default-blob-container-name");
                XElement defaultQueueNameElement = element.Element("default-queue-name");
                XElement defaultTableNameElement = element.Element("default-table-name");
                XElement defaultTableData = element.Element("table-data");
                XElement defaultLeaseBlockNameElement = element.Element("default-lease-block-name");
                XElement defaultSubscriptionNameElement = element.Element("default-subscription-name");
                XElement defaultTopicNameElement = element.Element("default-topic-name");
                XElement defaultBrokeredMessageQueueNameElement = element.Element("default-brokered-message-queue-name");
                XElement settingsElement = element.Element("settings");
                XAttribute defaultBlobContainerAccessAttribute = defaultBlobContainerNameElement?.Attribute("public-permission");

                if (sqlServerElement != null)
                {
                    try
                    {
                        component.SqlServerConnectionString = configuration.SqlServerConnectionStrings[sqlServerElement.Value]; component.SqlServerConnectionString = configuration.SqlServerConnectionStrings[sqlServerElement.Value];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new InvalidDataException($"Sql server with fqn of {sqlServerElement.Value} is missing from configuration file.");
                    }
                    
                }
                if (storageElement != null)
                {
                    try
                    {
                        component.StorageAccountConnectionString = configuration.StorageAccounts[storageElement.Value].ConnectionString;
                    }
                    catch (Exception)
                    {
                        throw new InvalidDataException($"Storage account with fqn of {storageElement.Value} is missing from configuration file.");
                    }
                }
                if (serviceBusElement != null)
                {
                    try
                    {
                        component.ServiceBusConnectionString = configuration.ServiceBusConnectionStrings[serviceBusElement.Value];
                    }
                    catch (Exception)
                    {
                        throw new InvalidDataException($"Service bus account with fqn of {serviceBusElement.Value} is missing from configuration file.");
                    }
                }

                component.DbContextType = dbContextTypeElement?.Value;
                component.DefaultBlobContainerName = defaultBlobContainerNameElement?.Value;
                component.DefaultQueueName = defaultQueueNameElement?.Value;
                component.DefaultTableName = defaultTableNameElement?.Value;
                component.DefaultBlobContainerAccessType = BlobContainerPublicAccessTypeEnum.Off;
                component.DefaultLeaseBlockName = defaultLeaseBlockNameElement?.Value;
                component.DefaultTopicName = defaultTopicNameElement?.Value;
                component.DefaultSubscriptionName = defaultSubscriptionNameElement?.Value;
                component.DefaultBrokeredMessageQueueName = defaultBrokeredMessageQueueNameElement?.Value;
                component.TableData = defaultTableData?.Value;
                component.Uploads = element.Elements("upload").Select(x => x.Value).ToList();
                if (defaultBlobContainerAccessAttribute != null)
                {
                    string accessAttribtueValue = defaultBlobContainerAccessAttribute.Value.ToLower();
                    if (accessAttribtueValue == "blob")
                    {
                        component.DefaultBlobContainerAccessType = BlobContainerPublicAccessTypeEnum.Blob;
                    }
                    else if (accessAttribtueValue == "container")
                    {
                        component.DefaultBlobContainerAccessType = BlobContainerPublicAccessTypeEnum.Container;
                    }
                }

                settingsElement?.Elements().ToList().ForEach(x =>
                {
                    string resourceType = null;
                    XAttribute resourceTypeAttr = x.Attribute("resource-type");
                    if (resourceTypeAttr != null)
                    {
                        resourceType = resourceTypeAttr.Value;
                    }
                    Dictionary<string, string> attributeDictionary = x.Attributes().ToDictionary(attribute => attribute.Name.LocalName, attribute => attribute.Value);
                    component.Settings.Add(new ApplicationComponentSetting
                    {
                        Key = x.Name.LocalName,
                        ResourceType = resourceType,
                        Value = x.Value,
                        Attributes = attributeDictionary
                    });                        
                });

                configuration.ApplicationComponents.Add(component);
            });

            return configuration;
        }
    }
}
