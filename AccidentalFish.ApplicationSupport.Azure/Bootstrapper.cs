using AccidentalFish.ApplicationSupport.Azure.Alerts.Implementation;
using AccidentalFish.ApplicationSupport.Azure.Blobs;
using AccidentalFish.ApplicationSupport.Azure.Components;
using AccidentalFish.ApplicationSupport.Azure.Components.Implementation;
using AccidentalFish.ApplicationSupport.Azure.NoSql;
using AccidentalFish.ApplicationSupport.Azure.Policies;
using AccidentalFish.ApplicationSupport.Azure.Queues;
using AccidentalFish.ApplicationSupport.Azure.Runtime;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Alerts;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;

namespace AccidentalFish.ApplicationSupport.Azure
{
    /// <summary>
    /// Registers Azure specific implementations of interfaces exposed by AccidentalFish.ApplicationSupport.Core
    /// </summary>
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IDependencyResolver dependencyResolver)
        {
            RegisterDependencies(dependencyResolver, false, true, false);
        }

        public static void RegisterDependencies(IDependencyResolver dependencyResolver, bool forceAppConfig, bool useAzureSqlDatabaseConfiguration, bool useLegacyQueueSerializer)
        {
            // internal
            dependencyResolver.Register<ITableStorageQueryBuilder, TableStorageQueryBuilder>();
            dependencyResolver.Register<ITableContinuationTokenSerializer, TableContinuationTokenSerializer>();
            if (useLegacyQueueSerializer)
            {
                dependencyResolver.Register<IQueueSerializer, LegacyQueueSerializer>();
            }
            else
            {
                dependencyResolver.Register<IQueueSerializer, QueueSerializer>();
            }

            // configuration
            dependencyResolver.RegisterInstance<IConfiguration>(new Configuration.Configuration(forceAppConfig));

            // policies            
            dependencyResolver.Register<ILeaseManagerFactory, LeaseManagerFactory>();

            // runtime
            dependencyResolver.Register<IRuntimeEnvironment, RuntimeEnvironment>();
            
            // repositories and data storage
            dependencyResolver.Register<IQueueFactory, QueueFactory>();
            dependencyResolver.Register<IBlobRepositoryFactory, BlobRepositoryFactory>();
            dependencyResolver.Register<ITableStorageRepositoryFactory, TableStorageRepositoryFactory>();
            dependencyResolver.Register<ITableStorageConcurrencyManager, TableStorageConcurrencyManager>();

            // alerts
            dependencyResolver.Register<IAlertSender, AlertSender>();

            dependencyResolver.Register<IAzureApplicationResourceFactory, AzureApplicationResourceFactory>();
        }        
    }
}
