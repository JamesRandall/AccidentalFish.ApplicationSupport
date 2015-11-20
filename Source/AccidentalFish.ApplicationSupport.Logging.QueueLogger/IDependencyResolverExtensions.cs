using System;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger
{
    // ReSharper disable once InconsistentNaming
    public static class IDependencyResolverExtensions
    {
        public static IDependencyResolver UseQueueLogger(this IDependencyResolver dependencyResolver,
            Type loggerExtension = null,
            LogLevelEnum defaultMinimumLogLevel = LogLevelEnum.Warning)
        {
            if (loggerExtension == null)
            {
                loggerExtension = typeof (NullQueueLoggerExtension);
            }

            return dependencyResolver
                .Register(typeof (IQueueLoggerExtension), loggerExtension)
                .Register(() => GetLoggerFactory(dependencyResolver, defaultMinimumLogLevel))
                .Register(() => GetLoggerFactory(dependencyResolver, defaultMinimumLogLevel).CreateLogger())
                .Register(() => GetLoggerFactory(dependencyResolver, defaultMinimumLogLevel).CreateAsynchronousLogger());
        }

        private static ILoggerFactory GetLoggerFactory(IDependencyResolver dependencyResolver,
            LogLevelEnum defaultMinimumLogLevel)
        {
            IRuntimeEnvironment runtimeEnvironment = dependencyResolver.Resolve<IRuntimeEnvironment>();
            IApplicationResourceFactory applicationResourceFactory = dependencyResolver.Resolve<IApplicationResourceFactory>();
            IQueueLoggerExtension queueLoggerExtension = dependencyResolver.Resolve<IQueueLoggerExtension>();
            ICorrelationIdProvider correlationIdProvider = dependencyResolver.Resolve<ICorrelationIdProvider>();

            return new QueueLoggerFactory(runtimeEnvironment, applicationResourceFactory, queueLoggerExtension,
                correlationIdProvider, defaultMinimumLogLevel);
        }
    }
}
