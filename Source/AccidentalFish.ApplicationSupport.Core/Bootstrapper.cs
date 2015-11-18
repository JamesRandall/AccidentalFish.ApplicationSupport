using System;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Blobs.Implementation;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Components.Implementation;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.Core.Email.Implementation;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Implementation;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Policies.Implementation;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Queues.Implementation;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Core.Repository.Implementation;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Core.Runtime.Implementation;
using AccidentalFish.ApplicationSupport.Core.Threading;
using AccidentalFish.ApplicationSupport.Core.Threading.Implementation;
using AccidentalFish.ApplicationSupport.DependencyResolver;

namespace AccidentalFish.ApplicationSupport.Core
{
    /// <summary>
    /// Registers infrastructure and dependencies with a unity container
    /// Note that this generally is coupled with the bootstrapper found in the assembly
    /// AccidentalFish.ApplicationSupport.Azure that provides Azure specific implementations.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Register the dependencies in a container
        /// </summary>
        /// <param name="container">The container to use</param>
        /// <param name="correlationIdKey">The correlation ID key. Defaults to correlation-id</param>
        public static IDependencyResolver UseCore(this IDependencyResolver container,
            string correlationIdKey = "correlation-id")
        {
            container.Register<IBackoffPolicy, BackoffPolicy>();
            container.Register<ILeasedRetry, LeasedRetry>();
            container.Register<IAsynchronousBackoffPolicy, AsynchronousBackoffPolicy>();
            container.Register<IWaitHandle, ManualResetEventWaitHandle>();
            container.Register<IApplicationResourceSettingNameProvider, ApplicationResourceSettingNameProvider>();
            container.Register<IApplicationResourceFactory, ApplicationResourceFactory>();
            container.Register<IApplicationResourceSettingProvider, ApplicationResourceSettingProvider>();
            container.Register<IQueueFactory, NotSupportedQueueFactory>();
            container.Register<ILeaseManagerFactory, NotSupportedLeaseManagerFactory>();
            container.Register<IBlobRepositoryFactory, NotSupportedBlobRepositoryFactory>();

            if (!string.IsNullOrWhiteSpace(correlationIdKey))
            {
                container.RegisterInstance<ICorrelationIdProvider>(new CallContextCorrelationIdProvider(correlationIdKey));
            }
            else
            {
                container.RegisterInstance<ICorrelationIdProvider>(new NullCorrelationIdProvider());
            }

            container.Register<ILoggerFactory, ConsoleLoggerFactory>();
            
            container.Register<IComponentHost, ComponentHost>();
            container.Register<IEmailQueueDispatcher, EmailQueueDispatcher>();
            container.Register<IUnitOfWorkFactoryProvider, NotSupportedUnitOfWorkFactoryProvider>();
            container.Register<IRuntimeEnvironment, DefaultRuntimeEnvironment>();
            container.RegisterInstance<IConfiguration>(new DefaultConfiguration());

            container.RegisterInstance<IComponentFactory>(new ComponentFactory(container));
            container.Register<IComponentHostRestartHandler, DefaultComponentHostRestartHandler>();

            return container;
        }
    }
}
