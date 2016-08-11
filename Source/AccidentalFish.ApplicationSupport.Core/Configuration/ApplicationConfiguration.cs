using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using AccidentalFish.ApplicationSupport.Core.Components;

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
        /// Secrets associated with this configuration
        /// </summary>
        public IReadOnlyCollection<string> Secrets { get; set; }

        /// <summary>
        /// Loads the application configuration from an XML file
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <param name="settings">An optional settings file</param>
        /// <param name="checkForMissingSettings">If set to true then any missing settings generate an exception</param>
        /// <param name="applicationSecretStore">Optional secret store to use in addition to the settings</param>
        /// <param name="verboseLogger">Optional verbose logger</param>
        /// <returns></returns>
        public static async Task<ApplicationConfiguration> FromFileAsync(string filename, ApplicationConfigurationSettings settings,
            bool checkForMissingSettings, IConfiguration applicationSecretStore=null, Action<string> verboseLogger = null)
        {
            XDocument document;
            using (StreamReader reader = new StreamReader(filename))
            {
                document = XDocument.Load(reader);
            }
            return await FromXDocumentAsync(document, settings, checkForMissingSettings, applicationSecretStore, verboseLogger);
        }

        /// <summary>
        /// Loads the application configuration from an XML document
        /// </summary>
        /// <param name="document">The document</param>
        /// <param name="settings">An optional settings file</param>
        /// <param name="checkForMissingSettings">If set to true then any missing settings generate an exception</param>
        /// <param name="applicationSecretStore">Optional secret store to use in addition to the settings</param>
        /// <param name="verboseLogger">Optional verbose logger</param>
        /// <returns>An application configuration</returns>
        public static async Task<ApplicationConfiguration> FromXDocumentAsync(XDocument document, ApplicationConfigurationSettings settings,
            bool checkForMissingSettings, IConfiguration applicationSecretStore = null, Action<string> verboseLogger = null)
        {
            if (document.Root == null) return null;
            HashSet<string> secrets = new HashSet<string>();
            
            verboseLogger?.Invoke("Processing settings");
            ApplicationConfiguration configuration = new ApplicationConfiguration();
            IApplicationResourceSettingNameProvider nameProvider = new ApplicationResourceSettingNameProvider();
            IEnumerable<XElement> allDescendants = document.Descendants();
            Regex settingPattern = new Regex(@"(?:\{\{)([^}]*)(?:\}\})");
            foreach (XElement element in allDescendants)
            {
                if (!element.HasElements)
                {
                    Match match = settingPattern.Match(element.Value);
                    string value = element.Value;
                    bool containsSecret = false;
                    while (match.Success)
                    {
                        string settingName = match.Groups[1].Value;
                        ApplicationConfigurationSetting setting;
                        if (!settings.Settings.TryGetValue(settingName, out setting) && checkForMissingSettings)
                        {
                            throw new MissingSettingException();
                        }
                        if (setting != null)
                        {
                            containsSecret |= setting.IsSecret;
                            value = value.Replace($"{{{{{settingName}}}}}", setting.Value);
                        }
                        match = match.NextMatch();
                    }
                    element.Value = value;
                    if (containsSecret)
                    {
                        secrets.Add(value);
                    }
                }
            }
            
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

            foreach (XElement element in document.Root.Elements("component").ToList())
            {
                ApplicationComponent component = new ApplicationComponent
                {
                    Fqn = element.Attribute("fqn").Value
                };
                IComponentIdentity componentIdentity = new ComponentIdentity(component.Fqn);
                verboseLogger?.Invoke($"Parsing component {componentIdentity}");

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
                        string secret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.SqlConnectionString(componentIdentity)) : null;
                        component.SqlServerConnectionString = secret ?? configuration.SqlServerConnectionStrings[sqlServerElement.Value];
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
                        string secret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.StorageAccountConnectionString(componentIdentity)) : null;
                        component.StorageAccountConnectionString = secret ?? configuration.StorageAccounts[storageElement.Value].ConnectionString;
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
                        verboseLogger?.Invoke($"Looking for service bus connection string for component {componentIdentity}");
                        string secret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.ServiceBusConnectionString(componentIdentity)) : null;                        
                        if (secret != null)
                        {
                            verboseLogger?.Invoke($"Using secret store service bus connection string for component {componentIdentity}");
                        }
                        component.ServiceBusConnectionString = secret ?? configuration.ServiceBusConnectionStrings[serviceBusElement.Value];
                    }
                    catch (Exception)
                    {
                        throw new InvalidDataException($"Service bus account with fqn of {serviceBusElement.Value} is missing from configuration file.");
                    }
                }

                verboseLogger?.Invoke("1");

                string name = nameProvider.SqlContextType(componentIdentity);
                verboseLogger?.Invoke(name);
                string dbContextTypeSecret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(name) : null;
                verboseLogger?.Invoke("1.1");
                if (!string.IsNullOrWhiteSpace(dbContextTypeSecret)) verboseLogger?.Invoke($"Using secret store for dbContextType for component {componentIdentity}");
                verboseLogger?.Invoke("1.2");
                component.DbContextType = dbContextTypeSecret ?? dbContextTypeElement?.Value;

                verboseLogger?.Invoke("2");

                string defaultBlobContainerNameSecret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.DefaultBlobContainerName(componentIdentity)) : null;
                if (!string.IsNullOrWhiteSpace(defaultBlobContainerNameSecret)) verboseLogger?.Invoke($"Using secret store for default blob container name for component {componentIdentity}");
                component.DefaultBlobContainerName = defaultBlobContainerNameSecret ?? defaultBlobContainerNameElement?.Value;

                verboseLogger?.Invoke("3");

                string defaultQueueNameSecret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.DefaultQueueName(componentIdentity)) : null;
                if (!string.IsNullOrWhiteSpace(defaultQueueNameSecret)) verboseLogger?.Invoke($"Using secret store for default queue name for component {componentIdentity}");
                component.DefaultQueueName = defaultQueueNameSecret ?? defaultQueueNameElement?.Value;

                verboseLogger?.Invoke("4");

                string defaultTableNameSecret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.DefaultTableName(componentIdentity)) : null;
                if (!string.IsNullOrWhiteSpace(defaultTableNameSecret)) verboseLogger?.Invoke($"Using secret store for default table name for component {componentIdentity}");
                component.DefaultTableName = defaultTableNameSecret ?? defaultTableNameElement?.Value;

                component.DefaultBlobContainerAccessType = BlobContainerPublicAccessTypeEnum.Off;

                verboseLogger?.Invoke("5");

                string defaultLeaseBlockNameSecret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.DefaultLeaseBlockName(componentIdentity)) : null;
                if (!string.IsNullOrWhiteSpace(defaultLeaseBlockNameSecret)) verboseLogger?.Invoke($"Using secret store for default lease block name for component {componentIdentity}");
                component.DefaultLeaseBlockName = defaultLeaseBlockNameSecret ?? defaultLeaseBlockNameElement?.Value;

                verboseLogger?.Invoke("6");

                string defaultTopicNameSecret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.DefaultTopicName(componentIdentity)) : null;
                if (!string.IsNullOrWhiteSpace(defaultTopicNameSecret)) verboseLogger?.Invoke($"Using secret store for default topic name for component {componentIdentity}");
                component.DefaultTopicName = defaultTopicNameSecret ?? defaultTopicNameElement?.Value;

                verboseLogger?.Invoke("7");

                string defaultSubscriptionNameSecret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.DefaultSubscriptionName(componentIdentity)) : null;
                if (!string.IsNullOrWhiteSpace(defaultSubscriptionNameSecret)) verboseLogger?.Invoke($"Using secret store for default subscription name for component {componentIdentity}");
                component.DefaultSubscriptionName = defaultSubscriptionNameSecret ?? defaultSubscriptionNameElement?.Value;

                verboseLogger?.Invoke("8");

                string defaultBrokeredMessageQueueNameSecret = applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.DefaultBrokeredMessageQueueName(componentIdentity)) : null;
                if (!string.IsNullOrWhiteSpace(defaultBrokeredMessageQueueNameSecret)) verboseLogger?.Invoke($"Using secret store for default brokered message queue name for component {componentIdentity}");
                component.DefaultBrokeredMessageQueueName = defaultBrokeredMessageQueueNameSecret ?? defaultBrokeredMessageQueueNameElement?.Value;

                verboseLogger?.Invoke("9");

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

                verboseLogger?.Invoke($"Processing settings for {componentIdentity}");
                if (settingsElement != null)
                {
                    foreach (XElement componentSettingsElement in settingsElement.Elements().ToList())
                    {
                        string resourceType = null;
                        XAttribute resourceTypeAttr = componentSettingsElement.Attribute("resource-type");
                        if (resourceTypeAttr != null)
                        {
                            resourceType = resourceTypeAttr.Value;
                        }
                        Dictionary<string, string> attributeDictionary = componentSettingsElement.Attributes().ToDictionary(attribute => attribute.Name.LocalName, attribute => attribute.Value);
                        component.Settings.Add(new ApplicationComponentSetting
                        {
                            Key = componentSettingsElement.Name.LocalName,
                            ResourceType = resourceType,
                            Value = (applicationSecretStore != null ? await applicationSecretStore.GetAsync(nameProvider.SettingName(componentIdentity, componentSettingsElement.Name.LocalName)) : null) ?? componentSettingsElement.Value,
                            Attributes = attributeDictionary
                        });
                    }
                }
                
                configuration.ApplicationComponents.Add(component);
                verboseLogger?.Invoke($"Finished parsing component {componentIdentity}");
            }

            configuration.Secrets = secrets.ToList();

            return configuration;
        }
    }
}
