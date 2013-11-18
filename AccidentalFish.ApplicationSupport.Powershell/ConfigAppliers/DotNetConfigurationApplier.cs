using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;

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

        public void Apply(ApplicationConfiguration configuration, string targetFile)
        {
            XDocument dotnetConfig;

            using (StreamReader reader = new StreamReader(targetFile))
            {
                dotnetConfig = XDocument.Load(reader);
            }
            
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
                    ApplyAppSetting(appSettings, key, component.SqlServerConnectionString);
                }
                if (!string.IsNullOrWhiteSpace(component.StorageAccountConnectionString))
                {
                    string key = _nameProvider.StorageAccountConnectionString(componentIdentity);
                    ApplyAppSetting(appSettings, key, component.StorageAccountConnectionString);
                }
                if (!string.IsNullOrWhiteSpace(component.DbContextType))
                {
                    string key = _nameProvider.SqlContextType(componentIdentity);
                    ApplyAppSetting(appSettings, key, component.DbContextType);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                {
                    string key = _nameProvider.DefaultQueueName(componentIdentity);
                    ApplyAppSetting(appSettings, key, component.DefaultQueueName);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                {
                    string key = _nameProvider.DefaultBlobContainerName(componentIdentity);
                    ApplyAppSetting(appSettings, key, component.DefaultBlobContainerName);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                {
                    string key = _nameProvider.DefaultTableName(componentIdentity);
                    ApplyAppSetting(appSettings, key, component.DefaultTableName);
                }

                foreach (ApplicationComponentSetting setting in component.Settings)
                {
                    string key = _nameProvider.SettingName(componentIdentity, setting.Key);
                    ApplyAppSetting(appSettings, key, setting.Value);
                }
            }

            using (FileStream outputStream = new FileStream(targetFile, FileMode.Create))
            {
                dotnetConfig.Save(outputStream);
            }
            
        }

        private void ApplyAppSetting(XElement appSettings, string key, string value)
        {
            string xpath = String.Format("add[@key='{0}']", key);
            XElement appSetting = appSettings.XPathSelectElement(xpath);
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
