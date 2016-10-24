using AccidentalFish.ApplicationSupport.Azure.Alerts.Implementation;
using AccidentalFish.ApplicationSupport.Azure.Blobs;
using AccidentalFish.ApplicationSupport.Azure.Components;
using AccidentalFish.ApplicationSupport.Azure.Components.Implementation;
using AccidentalFish.ApplicationSupport.Azure.Configuration;
using AccidentalFish.ApplicationSupport.Azure.Implementation;
using AccidentalFish.ApplicationSupport.Azure.KeyVault;
using AccidentalFish.ApplicationSupport.Azure.KeyVault.Implementation;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Azure.Logging.Implementation;
using AccidentalFish.ApplicationSupport.Azure.NoSql;
using AccidentalFish.ApplicationSupport.Azure.Policies;
using AccidentalFish.ApplicationSupport.Azure.Queues;
using AccidentalFish.ApplicationSupport.Azure.Runtime;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation;
using AccidentalFish.ApplicationSupport.Core.Alerts;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.Azure
{
    /// <summary>
    /// Registers Azure specific implementations of interfaces exposed by AccidentalFish.ApplicationSupport.Core
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Use key vault for application configuration. This provides a secure way of retrieving secrets at runtime (connection strings, passwords etc.)
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver</param>
        /// <param name="clientId">Client ID of the Azure AD application associated with the key vault (must be granted read access to secrets)</param>
        /// <param name="clientSecret">Client secret of the Azure AD application associated with the key vault (must be granted read access to secrets)</param>
        /// <param name="vaultUri">The URI of the key vault e.g. https://mykeyvault.vault.azure.net</param>
        /// <param name="useKeyVaultExclusively">Defaults to false in which case only application keys not found in the local configuration (app settings, cscfg etc.) will be looked up in the vault. True if everything should be looked up in the vault.</param>
        /// <param name="checkIfKeyVaultKeyExistsBeforeGet">If true then this checks if the key exists in the vault before attempting a get. This is expensive but currently helps with Powershell sync context / message pump issues.</param>
        /// <returns>The dependency resolver confiugred with a key vault used for application configuration</returns>
        public static IDependencyResolver UseKeyVaultApplicationConfiguration(this IDependencyResolver dependencyResolver,
            string clientId,
            string clientSecret,
            string vaultUri,
            bool useKeyVaultExclusively=false,
            bool checkIfKeyVaultKeyExistsBeforeGet=false)
        {
            IConfiguration existingConfiguration = null;
            if (!useKeyVaultExclusively)
            {
                existingConfiguration = dependencyResolver.Resolve<IConfiguration>();
            }
            IConfiguration keyVaultConfiguration = new KeyVaultConfiguration(
                new KeyVault.Implementation.KeyVault(clientId, clientSecret, vaultUri, checkIfKeyVaultKeyExistsBeforeGet),
                dependencyResolver.Resolve<IKeyVaultConfigurationKeyEncoder>(),
                existingConfiguration);
            dependencyResolver.Register(() => keyVaultConfiguration);

            return dependencyResolver;
        }

        /// <summary>
        /// Use key vault for application configuration. This provides a secure way of retrieving secrets at runtime (connection strings, passwords etc.)
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver</param>
        /// <param name="clientId">Client ID of the Azure AD application associated with the key vault (must be granted read access to secrets)</param>
        /// <param name="clientSecret">Client secret of the Azure AD application associated with the key vault (must be granted read access to secrets)</param>
        /// <param name="vaultUri">The URI of the key vault e.g. https://mykeyvault.vault.azure.net</param>
        /// <param name="useKeyVaultExclusively">Defaults to false in which case only application keys not found in the local configuration (app settings, cscfg etc.) will be looked up in the vault. True if everything should be looked up in the vault.</param>
        /// <param name="checkIfKeyVaultKeyExistsBeforeGet">If true then this checks if the key exists in the vault before attempting a get. This is expensive but currently helps with Powershell sync context / message pump issues.</param>
        /// <param name="cachePolicy">The cache policy, null for the default policy</param>
        /// <returns>The dependency resolver confiugred with a key vault used for application configuration</returns>
        public static IDependencyResolver UseAsyncKeyVaultApplicationConfiguration(this IDependencyResolver dependencyResolver,
            string clientId,
            string clientSecret,
            string vaultUri,
            bool useKeyVaultExclusively = false,
            KeyVaultConfigurationCachePolicy cachePolicy = null,
            bool checkIfKeyVaultKeyExistsBeforeGet = false)
        {
            if (cachePolicy == null)
            {
                cachePolicy = KeyVaultConfigurationCachePolicy.Default;
            }

            IAsyncConfiguration existingConfiguration = null;
            if (!useKeyVaultExclusively)
            {
                existingConfiguration = dependencyResolver.Resolve<IAsyncConfiguration>();
            }
            IAsyncConfiguration keyVaultConfiguration = new AsyncKeyVaultConfiguration(
                new KeyVault.Implementation.KeyVault(clientId, clientSecret, vaultUri, checkIfKeyVaultKeyExistsBeforeGet),
                dependencyResolver.Resolve<IKeyVaultConfigurationKeyEncoder>(),
                cachePolicy,
                existingConfiguration);
            dependencyResolver.Register(() => keyVaultConfiguration);

            return dependencyResolver;
        }

        public static IDependencyResolver UseAzure(this IDependencyResolver dependencyResolver)
        {
            return UseAzure(dependencyResolver, false, true, false, false);
        }

        public static IDependencyResolver UseAzure(this IDependencyResolver dependencyResolver,
            bool forceAppConfig,
            bool useAzureSqlDatabaseConfiguration,
            bool useLegacyQueueSerializer,
            bool registerEmailAlertSender)
        {
            return dependencyResolver
                .Register<IKeyVaultFactory, KeyVaultFactory>()
                .Register<IKeyVaultConfigurationKeyEncoder, KeyVaultConfigurationKeyEncoder>()
                .Register<ITableStorageQueryBuilder, TableStorageQueryBuilder>()
                .Register<ITableContinuationTokenSerializer, TableContinuationTokenSerializer>()
                .Register(typeof (IQueueSerializer),
                    useLegacyQueueSerializer ? typeof (LegacyQueueSerializer) : typeof (QueueSerializer))
                .Register<IConfiguration>(() => new Configuration.Configuration(forceAppConfig))
                .Register<IAsyncConfiguration>(() => new AsyncConfiguration(forceAppConfig))
                // policies            
                .Register<ILeaseManagerFactory, LeaseManagerFactory>()
                // runtime
                .Register<IRuntimeEnvironment, RuntimeEnvironment>()
                // repositories and data storage
                .Register<IQueueFactory, QueueFactory>()
                .Register<IBlobRepositoryFactory, BlobRepositoryFactory>()
                .Register<ITableStorageRepositoryFactory, TableStorageRepositoryFactory>()
                .Register<ITableStorageConcurrencyManager, TableStorageConcurrencyManager>()
                .Register<IAzureQueueFactory, AzureQueueFactory>()
                // alerts
                .Register(typeof (IAlertSender),
                    registerEmailAlertSender ? typeof (AlertSender) : typeof (NullAlertSender))
                // azure
                .Register<IAzureResourceManager, AzureResourceManager>()
                .Register<IAzureApplicationResourceFactory, AzureApplicationResourceFactory>()
                .Register<IAsyncAzureApplicationResourceFactory, AsyncAzureApplicationResourceFactory>()
                // internal
                .Register<IAzureAssemblyLogger>(() => new AzureAssemblyLogger(
                    dependencyResolver.Resolve<ILoggerFactory>()
                        .CreateLogger(new LoggerSource("AccidentalFish.ApplicationSupport.Azure"))));
        }     
    }
}
