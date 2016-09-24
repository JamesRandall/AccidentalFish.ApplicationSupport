using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Azure.Configuration;
using AccidentalFish.ApplicationSupport.Azure.KeyVault;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;
using Nito.AsyncEx;

namespace SetKeyVaultSecrets
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
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
            if (!File.Exists(options.Configuration))
            {
                throw new ConsoleAppException("Configuration file does not exist");
            }

            IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(new UnityContainer());
            dependencyResolver.UseCore().UseAzure();

            string[] settingsFiles = options.Settings.Split(',');

            ApplicationConfigurationSettings settings = ApplicationConfigurationSettings.FromFiles(settingsFiles);
            ApplicationConfiguration configuration = await ApplicationConfiguration.FromFileAsync(options.Configuration, settings, false);

            IKeyVaultFactory keyVaultFactory = dependencyResolver.Resolve<IKeyVaultFactory>();
            IKeyVault keyVault = keyVaultFactory.Create(options.KeyVaultClientId, options.KeyVaultClientKey, options.KeyVaultUri);
            IKeyVaultConfigurationKeyEncoder encoder = dependencyResolver.Resolve<IKeyVaultConfigurationKeyEncoder>();

            await SetSecrets(configuration, keyVault, encoder, options.Verbose);
        }

        private static async Task SetSecrets(ApplicationConfiguration configuration, IKeyVault secretStore, IKeyVaultConfigurationKeyEncoder encoder, bool verbose)
        {
            try
            {
                IApplicationResourceSettingNameProvider nameProvider = new ApplicationResourceSettingNameProvider();
                // set the secrets
                foreach (ApplicationComponent component in configuration.ApplicationComponents)
                {
                    IComponentIdentity componentIdentity = new ComponentIdentity(component.Fqn);
                    if (!string.IsNullOrWhiteSpace(component.SqlServerConnectionString))
                    {
                        string key = nameProvider.SqlConnectionString(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.SqlServerConnectionString);
                    }
                    if (!string.IsNullOrWhiteSpace(component.StorageAccountConnectionString))
                    {
                        string key = nameProvider.StorageAccountConnectionString(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.StorageAccountConnectionString);
                    }
                    if (!string.IsNullOrWhiteSpace(component.ServiceBusConnectionString))
                    {
                        string key = nameProvider.ServiceBusConnectionString(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.ServiceBusConnectionString);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DbContextType))
                    {
                        string key = nameProvider.SqlContextType(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.DbContextType);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultQueueName))
                    {
                        string key = nameProvider.DefaultQueueName(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.DefaultQueueName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultBlobContainerName))
                    {
                        string key = nameProvider.DefaultBlobContainerName(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.DefaultBlobContainerName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultTableName))
                    {
                        string key = nameProvider.DefaultTableName(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.DefaultTableName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultLeaseBlockName))
                    {
                        string key = nameProvider.DefaultLeaseBlockName(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.DefaultLeaseBlockName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultSubscriptionName))
                    {
                        string key = nameProvider.DefaultSubscriptionName(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.DefaultSubscriptionName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultTopicName))
                    {
                        string key = nameProvider.DefaultTopicName(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.DefaultTopicName);
                    }
                    if (!string.IsNullOrWhiteSpace(component.DefaultBrokeredMessageQueueName))
                    {
                        string key = nameProvider.DefaultBrokeredMessageQueueName(componentIdentity);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, component.DefaultBrokeredMessageQueueName);
                    }

                    foreach (ApplicationComponentSetting setting in component.Settings)
                    {
                        string key = nameProvider.SettingName(componentIdentity, setting.Key);
                        await SetValueInSecretStoreIfIsSecret(secretStore, encoder, verbose, configuration, key, setting.Value);
                    }
                }
            }
            catch (AggregateException aex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception ex in aex.InnerExceptions)
                {
                    sb.AppendLine(ex.Message);                    
                }
                throw new ConsoleAppException(sb.ToString(), aex);
            }

        }

        private static async Task SetValueInSecretStoreIfIsSecret(IKeyVault secretStore, IKeyVaultConfigurationKeyEncoder encoder, bool verbose, ApplicationConfiguration configuration, string key, string value)
        {
            if (configuration.Secrets.Contains(value))
            {
                string encodedKey = encoder.Encode(key);
                await secretStore.SetSecretAsync(encodedKey, value);

                if (verbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Secret set for key {0}", key);
                }
            }
        }
    }
}
