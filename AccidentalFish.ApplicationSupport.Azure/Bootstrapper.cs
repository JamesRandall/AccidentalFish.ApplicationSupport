using System.Data.Entity;
using AccidentalFish.ApplicationSupport.Azure.Blobs;
using AccidentalFish.ApplicationSupport.Azure.NoSql;
using AccidentalFish.ApplicationSupport.Azure.Policies;
using AccidentalFish.ApplicationSupport.Azure.Queues;
using AccidentalFish.ApplicationSupport.Azure.Runtime;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using Microsoft.Practices.Unity;

namespace AccidentalFish.ApplicationSupport.Azure
{
    /// <summary>
    /// Registers Azure specific implementations of interfaces exposed by AccidentalFish.ApplicationSupport.Core
    /// </summary>
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IUnityContainer container)
        {
            RegisterDependencies(container, false, true);
        }

        public static void RegisterDependencies(IUnityContainer container, bool forceAppConfig, bool useSqlDatabaseConfiguration)
        {
            // configuration
            container.RegisterType<IConfiguration, Configuration.Configuration>(new InjectionConstructor(forceAppConfig));

            // policies            
            container.RegisterType<IRetryPolicy, ServiceBusRetryPolicy>(RetryPolicyType.Queue);

            // runtime
            container.RegisterType<IRuntimeEnvironment, RuntimeEnvironment>();
            
            // repositories and data storage
            container.RegisterType<IQueueFactory, QueueFactory>();
            container.RegisterType<IBlobRepositoryFactory, BlobRepositoryFactory>();
            container.RegisterType<INoSqlRepositoryFactory, NoSqlRepositoryFactory>();
            container.RegisterType<INoSqlConcurrencyManager, NoSqlConcurrencyManager>();

            if (useSqlDatabaseConfiguration)
            {
                container.RegisterType<IDbConfiguration, SqlDatabaseConfiguration>();
                DbConfiguration.SetConfiguration(new SqlDatabaseConfiguration());
            }
            else
            {
                container.RegisterType<IDbConfiguration, NullDatabaseConfiguration>();
                DbConfiguration.SetConfiguration(new NullDatabaseConfiguration());
            }
        }        
    }
}
