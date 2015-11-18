using System;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger
{
    // ReSharper disable once InconsistentNaming
    public static class IDependencyResolverExtensions
    {
        public static IDependencyResolver UseQueueLogger(this IDependencyResolver dependencyResolver, Type loggerExtension = null)
        {
            if (loggerExtension == null)
            {
                loggerExtension = typeof (NullQueueLoggerExtension);
            }

            dependencyResolver.Register(typeof (IQueueLoggerExtension), loggerExtension);
            dependencyResolver.Register<ILoggerFactory, QueueLoggerFactory>();
            
            return dependencyResolver;
        }
    }
}
