using System;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Components.Implementation;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.Core.Email.Implementation;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Implementation;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Policies.Implementation;
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
        /// The logger type to use
        /// </summary>
        public enum LoggerTypeEnum
        {
            /// <summary>
            /// A queue based logger
            /// </summary>
            Queue,
            /// <summary>
            /// A console based logger
            /// </summary>
            Console
        }

        /// <summary>
        /// Register the dependencies in a container
        /// </summary>
        /// <param name="container">The container to use</param>
        /// <param name="loggerExtension">The log extension, if any, to use. Defaults to null - no extension</param>
        /// <param name="loggerType">The type of logger to start. Defaults to a queue based logger (which requires a queue implementation such as Azure)</param>
        /// <param name="correlationIdKey">The correlation ID key. Defaults to correlation-id</param>
        public static IDependencyResolver UseCore(this IDependencyResolver container,
            Type loggerExtension = null,
            LoggerTypeEnum loggerType = LoggerTypeEnum.Queue,
            string correlationIdKey = "correlation-id")
        {
            container.Register<IBackoffPolicy, BackoffPolicy>();
            container.Register<ILeasedRetry, LeasedRetry>();
            container.Register<IAsynchronousBackoffPolicy, AsynchronousBackoffPolicy>();
            container.Register<IWaitHandle, ManualResetEventWaitHandle>();
            container.Register<IApplicationResourceSettingNameProvider, ApplicationResourceSettingNameProvider>();
            container.Register<IApplicationResourceFactory, ApplicationResourceFactory>();
            container.Register<IApplicationResourceSettingProvider, ApplicationResourceSettingProvider>();

            if (!string.IsNullOrWhiteSpace(correlationIdKey))
            {
                container.RegisterInstance<ICorrelationIdProvider>(new CallContextCorrelationIdProvider(correlationIdKey));
            }
            else
            {
                container.RegisterInstance<ICorrelationIdProvider>(new NullCorrelationIdProvider());
            }

            if (loggerType == LoggerTypeEnum.Console)
            {
                container.Register<ILoggerFactory, ConsoleLoggerFactory>();
            }
            else
            {
                container.Register<ILoggerFactory, QueueLoggerFactory>();
            }

            container.Register<IComponentHost, ComponentHost>();
            container.Register<IEmailManager, EmailQueueDispatcher>();
            container.Register<IEmailQueueDispatcher, EmailQueueDispatcher>();
            container.Register<IUnitOfWorkFactoryProvider, NotSupportedUnitOfWorkFactoryProvider>();
            container.Register<IRuntimeEnvironment, DefaultRuntimeEnvironment>();
            container.RegisterInstance<IConfiguration>(new DefaultConfiguration());

            if (loggerExtension == null)
            {
                container.Register<ILoggerExtension, NullLoggerExtension>();
            }
            else
            {
                container.Register(typeof(ILoggerExtension), loggerExtension);
            }
            container.RegisterInstance<IComponentFactory>(new ComponentFactory(container));

            return container;
        }

        /// <summary>
        /// Register the dependencies in a container. The system is configured with:
        ///     * No log extension
        ///     * A queue based logger (a queue implementation is required such as Azure)
        ///     * A correlation ID of correlation-id
        /// </summary>
        /// <param name="container">The container to use</param>
        /// <returns>The container so a fluent API style of configuration can be used</returns>
        [Obsolete]
        public static void RegisterDependencies(IDependencyResolver container)
        {
            RegisterDependencies(container, null, LoggerTypeEnum.Queue, "correlation-id");
        }

        /// <summary>
        /// Register the dependencies in a container
        /// </summary>
        /// <param name="container">The container to use</param>
        /// <param name="loggerExtension">The log extension, if any, to use</param>
        /// <param name="loggerType">The type of logger to start</param>
        /// <param name="correlationIdKey">The correlation ID key</param>
        [Obsolete]
        public static void RegisterDependencies(
            IDependencyResolver container,
            Type loggerExtension,
            LoggerTypeEnum loggerType,
            string correlationIdKey)
        {
            container.UseCore(loggerExtension, loggerType, correlationIdKey);
        }
    }
}
