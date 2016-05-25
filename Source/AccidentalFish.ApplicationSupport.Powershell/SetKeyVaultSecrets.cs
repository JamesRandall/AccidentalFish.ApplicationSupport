using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Powershell.SecretStore;

namespace AccidentalFish.ApplicationSupport.Powershell
{
    [Cmdlet(VerbsCommon.Set, "KeyVaultSecrets")]
    public class SetKeyVaultSecrets : PSCmdlet
    {
        [Parameter(HelpMessage = "The application configuration file", Mandatory = true)]
        public string Configuration { get; set; }

        [Parameter(HelpMessage = "Settings file(s). If more than one file is specified they are combined.", Mandatory = true)]
        public string[] Settings { get; set; }

        [Parameter(HelpMessage = "Client ID for the key vault to use for secrets", Mandatory = false)]
        public string KeyVaultClientId { get; set; }

        [Parameter(HelpMessage = "Client key for the key vault to use for secrets", Mandatory = false)]
        public string KeyVaultClientKey { get; set; }

        [Parameter(HelpMessage = "Name of the key vault to use for secrets", Mandatory = false)]
        public string KeyVaultName { get; set; }

        protected override void ProcessRecord()
        {
            if (!File.Exists(Configuration))
            {
                throw new InvalidOperationException("Configuration file does not exist");
            }
            
            ApplicationConfigurationSettings settings = Settings != null && Settings.Length > 0 ? ApplicationConfigurationSettings.FromFiles(Settings) : null;
            ApplicationConfiguration configuration = ApplicationConfiguration.FromFile(Configuration, settings, false);

            ISecretStore secretStore = null;
            bool useKeyVault = !string.IsNullOrWhiteSpace(KeyVaultClientId) && !string.IsNullOrWhiteSpace(KeyVaultClientKey) && !string.IsNullOrWhiteSpace(KeyVaultName);
            if (useKeyVault)
            {
                secretStore = new KeyVaultSecretStore(KeyVaultName, KeyVaultClientId, KeyVaultClientKey);
            }

            SetSecrets(configuration, secretStore);
        }

        private void SetSecrets(ApplicationConfiguration configuration, ISecretStore secretStore)
        {
            IApplicationResourceSettingNameProvider nameProvider = new ApplicationResourceSettingNameProvider();
            // set the secrets
            foreach (ApplicationComponent component in configuration.ApplicationComponents)
            {
                IComponentIdentity componentIdentity = new ComponentIdentity(component.Fqn);
                if (!string.IsNullOrWhiteSpace(component.SqlServerConnectionString))
                {
                    string key = nameProvider.SqlConnectionString(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.SqlServerConnectionString);
                }
                if (!string.IsNullOrWhiteSpace(component.StorageAccountConnectionString))
                {
                    string key = nameProvider.StorageAccountConnectionString(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.StorageAccountConnectionString);
                }
                if (!string.IsNullOrWhiteSpace(component.ServiceBusConnectionString))
                {
                    string key = nameProvider.ServiceBusConnectionString(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.ServiceBusConnectionString);
                }
                if (!string.IsNullOrWhiteSpace(component.DbContextType))
                {
                    string key = nameProvider.SqlContextType(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.DbContextType);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                {
                    string key = nameProvider.DefaultQueueName(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.DefaultQueueName);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                {
                    string key = nameProvider.DefaultBlobContainerName(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.DefaultBlobContainerName);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                {
                    string key = nameProvider.DefaultTableName(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.DefaultTableName);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultLeaseBlockName))
                {
                    string key = nameProvider.DefaultLeaseBlockName(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.DefaultLeaseBlockName);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultSubscriptionName))
                {
                    string key = nameProvider.DefaultSubscriptionName(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.DefaultSubscriptionName);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultTopicName))
                {
                    string key = nameProvider.DefaultTopicName(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.DefaultTopicName);
                }
                if (!string.IsNullOrWhiteSpace(component.DefaultBrokeredMessageQueueName))
                {
                    string key = nameProvider.DefaultBrokeredMessageQueueName(componentIdentity);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, component.DefaultBrokeredMessageQueueName);
                }

                foreach (ApplicationComponentSetting setting in component.Settings)
                {
                    string key = nameProvider.SettingName(componentIdentity, setting.Key);
                    SetValueInSecretStoreIfIsSecret(secretStore, configuration, key, setting.Value);
                }
            }
        }

        private void SetValueInSecretStoreIfIsSecret(ISecretStore secretStore, ApplicationConfiguration configuration, string key, string value)
        {
            if (configuration.Secrets.Contains(value))
            {
                secretStore.Save(key, value).Wait();
            }
        }
    }
}
