using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;

namespace AccidentalFish.ApplicationSupport.Powershell.ConfigAppliers
{
    internal class CsdefConfigurationApplier : IConfigurationApplier
    {
        private readonly IApplicationResourceSettingNameProvider _nameProvider;
        private const string CsdefNamespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition";

        public CsdefConfigurationApplier() : this(new ApplicationResourceSettingNameProvider())
        {
            
        }

        public CsdefConfigurationApplier(IApplicationResourceSettingNameProvider nameProvider)
        {
            _nameProvider = nameProvider;
        }

        public void Apply(ApplicationConfiguration configuration, string targetFile)
        {
            XDocument csdef;
            XNamespace ns = CsdefNamespace;

            using (StreamReader reader = new StreamReader(targetFile))
            {
                csdef = XDocument.Load(reader);
            }

            XElement[] workerRoles = csdef.Root.Elements(ns + "WorkerRole").ToArray();
            foreach (XElement workerRole in workerRoles)
            {
                XElement configurationSettings = workerRole.Element(ns + "ConfigurationSettings");
                if (configurationSettings == null)
                {
                    configurationSettings = new XElement(ns + "ConfigurationSettings");
                    workerRole.Add(configurationSettings);
                }

                foreach (ApplicationComponent component in configuration.ApplicationComponents)
                {
                    IComponentIdentity componentIdentity = new ComponentIdentity(component.Fqn);
                    if (!string.IsNullOrWhiteSpace(component.SqlServerConnectionString))
                    {
                        string key = _nameProvider.SqlConnectionString(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.StorageAccountConnectionString))
                    {
                        string key = _nameProvider.StorageAccountConnectionString(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.ServiceBusConnectionString))
                    {
                        string key = _nameProvider.ServiceBusConnectionString(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DbContextType))
                    {
                        string key = _nameProvider.SqlContextType(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                    {
                        string key = _nameProvider.DefaultQueueName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                    {
                        string key = _nameProvider.DefaultBlobContainerName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                    {
                        string key = _nameProvider.DefaultTableName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultLeaseBlockName))
                    {
                        string key = _nameProvider.DefaultLeaseBlockName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultSubscriptionName))
                    {
                        string key = _nameProvider.DefaultSubscriptionName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultTopicName))
                    {
                        string key = _nameProvider.DefaultTopicName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key);
                    }

                    foreach (ApplicationComponentSetting setting in component.Settings)
                    {
                        string key = _nameProvider.SettingName(componentIdentity, setting.Key);
                        ApplyConfigSetting(configurationSettings, key);
                    }
                }
            }

            using (FileStream outputStream = new FileStream(targetFile, FileMode.Create))
            {
                csdef.Save(outputStream);
            }
        }

        private void ApplyConfigSetting(XElement configurationSettings, string key)
        {
            XNamespace ns = CsdefNamespace;
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("csdef", CsdefNamespace);
            string xpath = String.Format("csdef:Setting[@name='{0}']", key);
            XElement appSetting = configurationSettings.XPathSelectElement(xpath, namespaceManager);
            if (appSetting == null)
            {
                appSetting = new XElement(ns + "Setting", new XAttribute("name", key));
                configurationSettings.Add(appSetting);
            }
        }
    }
}
