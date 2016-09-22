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
using AccidentalFish.ApplicationSupport.Core.Naming;
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
        /// <param name="defaultTraceLoggerMinimumLogLevel"></param>
        /// <param name="defaultLoggerSource">The component source to use for a default logger when no source is specified - defaults to null.</param>
        public static IDependencyResolver UseCore(
            this IDependencyResolver container,
            string correlationIdKey = "correlation-id",
            LogLevelEnum defaultTraceLoggerMinimumLogLevel = LogLevelEnum.Warning,
            IFullyQualifiedName defaultLoggerSource = null)
        {
            Func<ICorrelationIdProvider> createCorrelationIdProvider;
            if (!string.IsNullOrWhiteSpace(correlationIdKey))
            {
                createCorrelationIdProvider = () => new CallContextCorrelationIdProvider(correlationIdKey);
            }
            else
            {
                createCorrelationIdProvider = () => new NullCorrelationIdProvider();
            }
            return container
                .Register<IAsynchronousDelay, AsynchronousDelay>()
                .Register<ITimerThreadPoolExecuter, TimerThreadPoolExecuter>()
                .Register<ITimerFactory, TimerFactory>()
                .Register<IBackoffPolicy, BackoffPolicy>()
                .Register<IBackoffPolicyTimingProvider, BackoffPolicyDefaultTimingProvider>()
                .Register<IAsynchronousBackoffPolicy, AsynchronousBackoffPolicy>()
                .Register<IBackoffPolicyFactory, BackoffPolicyFactory>()
                .Register<ILeasedRetry, LeasedRetry>()
                .Register<IWaitHandle, ManualResetEventWaitHandle>()
                .Register<IApplicationResourceSettingNameProvider, ApplicationResourceSettingNameProvider>()
                .Register<IAsyncApplicationResourceSettingProvider, AsyncApplicationResourceSettingProvider>()
                .Register<IAsyncApplicationResourceFactory, AsyncApplicationResourceFactory>()
                .Register<IApplicationResourceFactory, ApplicationResourceFactory>()
                .Register<IApplicationResourceSettingProvider, ApplicationResourceSettingProvider>()
                .Register<IQueueFactory, NotSupportedQueueFactory>()
                .Register<ILeaseManagerFactory, NotSupportedLeaseManagerFactory>()
                .Register<IBlobRepositoryFactory, NotSupportedBlobRepositoryFactory>()
                .Register(createCorrelationIdProvider)
                .Register<ILoggerFactory>(() => new TraceLoggerFactory(defaultTraceLoggerMinimumLogLevel, defaultLoggerSource))
                .Register(() => new TraceLoggerFactory(defaultTraceLoggerMinimumLogLevel, defaultLoggerSource).CreateLogger())
                .Register(() => new TraceLoggerFactory(defaultTraceLoggerMinimumLogLevel, defaultLoggerSource).CreateAsynchronousLogger(defaultTraceLoggerMinimumLogLevel))
                .Register<IComponentHost, ComponentHost>()
                .Register<IEmailQueueDispatcher, EmailQueueDispatcher>()
                .Register<IUnitOfWorkFactoryProvider, NotSupportedUnitOfWorkFactoryProvider>()
                .Register<IRuntimeEnvironment, DefaultRuntimeEnvironment>()
                .Register<IConfiguration>(() => new DefaultConfiguration())
                .Register<IAsyncConfiguration>(() => new DefaultAsyncConfiguration())
                .Register<IComponentFactory>(() => new ComponentFactory(container))
                .Register<IComponentHostRestartHandler, DefaultComponentHostRestartHandler>()
                .Register<ILargeMessageQueueFactory, LargeMessageQueueFactory>()                
                // internal
                .Register<ICoreAssemblyLogger>(() =>
                    new CoreAssemblyLogger(container.Resolve<ILoggerFactory>().CreateLogger(new LoggerSource("AccidentalFish.ApplicationSupport.Core"))));
        }
    }
}
