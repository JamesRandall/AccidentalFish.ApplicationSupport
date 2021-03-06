﻿using System;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger.Implementation;

namespace AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger
{
    // ReSharper disable once InconsistentNaming
    public static class IDependencyResolverExtensions
    {
        public static IDependencyResolver UseQueueLogger(this IDependencyResolver dependencyResolver,
            Type loggerExtension = null,
            LogLevelEnum defaultMinimumLogLevel = LogLevelEnum.Warning,
            IFullyQualifiedName defaultLoggerSource = null)
        {
            if (loggerExtension == null)
            {
                loggerExtension = typeof (NullQueueLoggerExtension);
            }

            return dependencyResolver
                .Register(typeof (IQueueLoggerExtension), loggerExtension)
                .Register(() => GetLoggerFactory(dependencyResolver, defaultMinimumLogLevel, defaultLoggerSource))
                .Register(() => GetLoggerFactory(dependencyResolver, defaultMinimumLogLevel, defaultLoggerSource).CreateLogger())
                .Register(() => GetLoggerFactory(dependencyResolver, defaultMinimumLogLevel, defaultLoggerSource).CreateAsynchronousLogger());
        }

        private static ILoggerFactory GetLoggerFactory(
            IDependencyResolver dependencyResolver,
            LogLevelEnum defaultMinimumLogLevel,
            IFullyQualifiedName defaultLoggerSource)
        {
            IRuntimeEnvironment runtimeEnvironment = dependencyResolver.Resolve<IRuntimeEnvironment>();
            IConfiguration configuration = dependencyResolver.Resolve<IConfiguration>();
            IQueueLoggerExtension queueLoggerExtension = dependencyResolver.Resolve<IQueueLoggerExtension>();
            ICorrelationIdProvider correlationIdProvider = dependencyResolver.Resolve<ICorrelationIdProvider>();
            IQueueSerializer queueSerializer = dependencyResolver.Resolve<IQueueSerializer>();
            IApplicationResourceSettingNameProvider nameProvider = dependencyResolver.Resolve<IApplicationResourceSettingNameProvider>();

            return new QueueLoggerFactory(runtimeEnvironment, nameProvider, configuration, queueSerializer, queueLoggerExtension,
                correlationIdProvider, defaultMinimumLogLevel, defaultLoggerSource);
        }
    }
}
