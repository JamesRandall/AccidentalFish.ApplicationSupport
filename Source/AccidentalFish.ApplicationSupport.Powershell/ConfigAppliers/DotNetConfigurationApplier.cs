using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Powershell.SecretStore;

namespace AccidentalFish.ApplicationSupport.Powershell.ConfigAppliers
{
    internal class DotNetConfigurationApplier : IConfigurationApplier
    {
        private readonly IApplicationResourceSettingNameProvider _nameProvider;

        public DotNetConfigurationApplier() : this(new ApplicationResourceSettingNameProvider())
        {
            
        }

        public DotNetConfigurationApplier(IApplicationResourceSettingNameProvider nameProvider)
        {
            _nameProvider = nameProvider;
        }

        public void Apply(ApplicationConfiguration configuration, ApplicationConfigurationSettings settings, string targetFile)
        {
            XDocument dotnetConfig;

            using (StreamReader reader = new StreamReader(targetFile))
            {
                dotnetConfig = XDocument.Load(reader);
            }

            if (dotnetConfig.Root != null)
            {
                XElement appSettings = dotnetConfig.Root.Element("appSettings");
                if (appSettings == null)
                {
                    throw new ApplicationException("There must be an appSettings section in your .config file.");
                }
            
                foreach (ApplicationComponent component in configuration.ApplicationComponents)
                {
                    IComponentIdentity componentIdentity = new ComponentIdentity(component.Fqn);
                    if (!string.IsNullOrWhiteSpace(component.SqlServerConnectionString))
                    {
                        string key = _nameProvider.SqlConnectionString(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.SqlServerConnectionString);
                    }
                    if (!string.IsNullOrWhiteSpace(component.StorageAccountConnectionString))
                    {
                        string key = _nameProvider.StorageAccountConnectionString(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.StorageAccountConnectionString);
                    }
                    if (!string.IsNullOrWhiteSpace(component.ServiceBusConnectionString))
                    {
                        string key = _nameProvider.ServiceBusConnectionString(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.ServiceBusConnectionString);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DbContextType))
                    {
                        string key = _nameProvider.SqlContextType(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.DbContextType);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                    {
                        string key = _nameProvider.DefaultQueueName(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.DefaultQueueName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                    {
                        string key = _nameProvider.DefaultBlobContainerName(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.DefaultBlobContainerName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                    {
                        string key = _nameProvider.DefaultTableName(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.DefaultTableName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultLeaseBlockName))
                    {
                        string key = _nameProvider.DefaultLeaseBlockName(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.DefaultLeaseBlockName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultSubscriptionName))
                    {
                        string key = _nameProvider.DefaultSubscriptionName(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.DefaultSubscriptionName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultTopicName))
                    {
                        string key = _nameProvider.DefaultTopicName(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.DefaultTopicName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultBrokeredMessageQueueName))
                    {
                        string key = _nameProvider.DefaultBrokeredMessageQueueName(componentIdentity);
                        ApplyAppSetting(configuration, appSettings, key, component.DefaultBrokeredMessageQueueName);
                    }

                    foreach (ApplicationComponentSetting setting in component.Settings)
                    {
                        string key = _nameProvider.SettingName(componentIdentity, setting.Key);
                        ApplyAppSetting(configuration, appSettings, key, setting.Value);
                    }
                }
            }

            using (FileStream outputStream = new FileStream(targetFile, FileMode.Create))
            {
                dotnetConfig.Save(outputStream);
            }
            
        }

        private void ApplyAppSetting(ApplicationConfiguration configuration, XElement appSettings, string key, string value)
        {
            string xpath = $"add[@key='{key}']";
            XElement appSetting = appSettings.XPathSelectElement(xpath);
            if (configuration.Secrets.Contains(value))
            {
                appSetting?.Remove();
            }
            else
            {                
                if (appSetting == null)
                {
                    appSetting = new XElement("add", new XAttribute("key", key), new XAttribute("value", value));
                    appSettings.Add(appSetting);
                }
                else
                {
                    appSetting.SetAttributeValue("value", value);
                }
            }
        }
    }
}
