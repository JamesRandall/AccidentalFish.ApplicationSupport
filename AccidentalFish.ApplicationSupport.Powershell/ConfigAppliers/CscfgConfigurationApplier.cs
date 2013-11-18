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
    internal class CscfgConfigurationApplier : IConfigurationApplier
    {
        private readonly IApplicationResourceSettingNameProvider _nameProvider;
        private const string CscfgNamespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration";

        public CscfgConfigurationApplier()
            : this(new ApplicationResourceSettingNameProvider())
        {

        }

        public CscfgConfigurationApplier(IApplicationResourceSettingNameProvider nameProvider)
        {
            _nameProvider = nameProvider;
        }

        public void Apply(ApplicationConfiguration configuration, string targetFile)
        {
            XDocument cscfg;
            XNamespace ns = CscfgNamespace;

            using (StreamReader reader = new StreamReader(targetFile))
            {
                cscfg = XDocument.Load(reader);
            }

            XElement[] workerRoles = cscfg.Root.Elements(ns + "Role").ToArray();
            foreach (XElement role in workerRoles)
            {
                XElement configurationSettings = role.Element(ns + "ConfigurationSettings");
                if (configurationSettings == null)
                {
                    configurationSettings = new XElement(ns + "ConfigurationSettings");
                    role.Add(configurationSettings);
                }

                foreach (ApplicationComponent component in configuration.ApplicationComponents)
                {
                    IComponentIdentity componentIdentity = new ComponentIdentity(component.Fqn);
                    if (!string.IsNullOrWhiteSpace(component.SqlServerConnectionString))
                    {
                        string key = _nameProvider.SqlConnectionString(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key, component.SqlServerConnectionString);
                    }
                    if (!string.IsNullOrWhiteSpace(component.StorageAccountConnectionString))
                    {
                        string key = _nameProvider.StorageAccountConnectionString(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key, component.StorageAccountConnectionString);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DbContextType))
                    {
                        string key = _nameProvider.SqlContextType(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key, component.DbContextType);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                    {
                        string key = _nameProvider.DefaultQueueName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key, component.DefaultQueueName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                    {
                        string key = _nameProvider.DefaultBlobContainerName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key, component.DefaultBlobContainerName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                    {
                        string key = _nameProvider.DefaultTableName(componentIdentity);
                        ApplyConfigSetting(configurationSettings, key, component.DefaultTableName);
                    }

                    foreach (ApplicationComponentSetting setting in component.Settings)
                    {
                        string key = _nameProvider.SettingName(componentIdentity, setting.Key);
                        ApplyConfigSetting(configurationSettings, key, setting.Value);
                    }
                }
            }

            using (FileStream outputStream = new FileStream(targetFile, FileMode.Create))
            {
                cscfg.Save(outputStream);
            }
        }

        private void ApplyConfigSetting(XElement configurationSettings, string key, string value)
        {
            XNamespace ns = CscfgNamespace;
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("cscfg", CscfgNamespace);
            string xpath = String.Format("cscfg:Setting[@name='{0}']", key);
            XElement appSetting = configurationSettings.XPathSelectElement(xpath, namespaceManager);
            if (appSetting == null)
            {
                appSetting = new XElement(ns + "Setting", new XAttribute("name", key), new XAttribute("value", value));
                configurationSettings.Add(appSetting);
            }
            else
            {
                appSetting.SetAttributeValue("value", value);
            }
        }
    }
}
