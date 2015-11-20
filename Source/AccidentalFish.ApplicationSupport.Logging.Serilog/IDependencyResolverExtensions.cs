using System;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Logging.Serilog.Implementation;
using Serilog;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog
{
    // ReSharper disable once InconsistentNaming
    public static class IDependencyResolverExtensions
    {
        /// <summary>
        /// Register Serilog as the frameworks default logger.
        /// </summary>
        /// <param name="dependencyResolver">Dependency resolver to register Serilog in</param>
        /// <param name="configurationProvider">A function that returns a SeriLog LoggerConfiguration class.
        /// If null then Serilog is configured to write using the Trace sink. Note that a LoggerConfiguration returned from this function
        /// will be further annotated with a minimum log level and property enrichment for correlation ID and source. If you don't want
        /// any of that then use the CreateSerilog(LoggerConfiguration loggerConfiguration) on ISerilogFactory</param>
        /// <param name="defaultMinimumLogLevel">The default minimum log level for loggers created by the factory</param>
        /// <param name="defaultLoggerSource">The component source to use for a logger when no source is specified - defaults to null.</param>
        /// <param name="sourceFqnPropertyName">The Serilog property name for the source component fully qualified name</param>
        /// <param name="correlationIdPropertyName">The Serilog property name for the correlation ID</param>
        /// <returns></returns>
        public static IDependencyResolver UseSerilog(this IDependencyResolver dependencyResolver,
            Func<LoggerConfiguration> configurationProvider = null,
            LogLevelEnum defaultMinimumLogLevel = LogLevelEnum.Warning,
            IFullyQualifiedName defaultLoggerSource = null,
            string sourceFqnPropertyName = "SourceFqn",
            string correlationIdPropertyName = "CorrelationId")
        {
            return dependencyResolver
                .Register<ILoggerFactory>(() => new SerilogFactory(configurationProvider, dependencyResolver.Resolve<ICorrelationIdProvider>(), defaultMinimumLogLevel, defaultLoggerSource, sourceFqnPropertyName, correlationIdPropertyName))
                .Register<ISerilogFactory>(() => new SerilogFactory(configurationProvider, dependencyResolver.Resolve<ICorrelationIdProvider>(), defaultMinimumLogLevel, defaultLoggerSource, sourceFqnPropertyName, correlationIdPropertyName))
                .Register(() => new SerilogFactory(configurationProvider, dependencyResolver.Resolve<ICorrelationIdProvider>(), defaultMinimumLogLevel, defaultLoggerSource, sourceFqnPropertyName, correlationIdPropertyName).CreateLogger())
                .Register(() => new SerilogFactory(configurationProvider, dependencyResolver.Resolve<ICorrelationIdProvider>(), defaultMinimumLogLevel, defaultLoggerSource, sourceFqnPropertyName, correlationIdPropertyName).CreateSerilog());
        }
    }
}
