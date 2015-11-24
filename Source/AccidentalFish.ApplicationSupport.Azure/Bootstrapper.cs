using System;
using AccidentalFish.ApplicationSupport.Azure.Alerts.Implementation;
using AccidentalFish.ApplicationSupport.Azure.Blobs;
using AccidentalFish.ApplicationSupport.Azure.Components;
using AccidentalFish.ApplicationSupport.Azure.Components.Implementation;
using AccidentalFish.ApplicationSupport.Azure.Implementation;
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
                .Register<ITableStorageQueryBuilder, TableStorageQueryBuilder>()
                .Register<ITableContinuationTokenSerializer, TableContinuationTokenSerializer>()
                .Register(typeof (IQueueSerializer),
                    useLegacyQueueSerializer ? typeof (LegacyQueueSerializer) : typeof (QueueSerializer))
                .Register<IConfiguration>(() => new Configuration.Configuration(forceAppConfig))
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
                // internal
                .Register<IAzureAssemblyLogger>(() => new AzureAssemblyLogger(
                    dependencyResolver.Resolve<ILoggerFactory>()
                        .CreateLogger(new LoggerSource("AccidentalFish.ApplicationSupport.Azure"))));
        }     
    }
}
