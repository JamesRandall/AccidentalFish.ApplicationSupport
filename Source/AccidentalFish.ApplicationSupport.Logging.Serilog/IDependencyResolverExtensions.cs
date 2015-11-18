using System;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Logging.Serilog.Implementation;
using Serilog;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog
{
    // ReSharper disable once InconsistentNaming
    public static class IDependencyResolverExtensions
    {
        public static IDependencyResolver UseSerilog(this IDependencyResolver dependencyResolver,
            Func<LoggerConfiguration> configurationProvider)
        {
            ICorrelationIdProvider correlationIdProvider = dependencyResolver.Resolve<ICorrelationIdProvider>();
            ILoggerFactory loggerFactory = new SerilogFactory(configurationProvider, correlationIdProvider);
            dependencyResolver.RegisterInstance(loggerFactory);
            return dependencyResolver;
        }
    }
}
