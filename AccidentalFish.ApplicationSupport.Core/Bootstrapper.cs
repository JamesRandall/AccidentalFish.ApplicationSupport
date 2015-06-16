using System;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Components.Implementation;
using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.Core.Email.Implementation;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Implementation;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Policies.Implementation;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Core.Runtime.Implementation;
using AccidentalFish.ApplicationSupport.Core.Threading;
using AccidentalFish.ApplicationSupport.Core.Threading.Implementation;
using AccidentalFish.ApplicationSupport.Injection;

namespace AccidentalFish.ApplicationSupport.Core
{
    /// <summary>
    /// Registers infrastructure and dependencies with a unity container
    /// Note that this generally is coupled with the bootstrapper found in the assembly
    /// AccidentalFish.ApplicationSupport.Azure that provides Azure specific implementations.
    /// </summary>
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IDependencyResolver container)
        {
            RegisterDependencies(container, null);
        }

        public static void RegisterDependencies(
            IDependencyResolver container,
            Type loggerExtension)
        {
            container.Register<IBackoffPolicy, BackoffPolicy>();
            container.Register<ILeasedRetry, LeasedRetry>();
            container.Register<IAsynchronousBackoffPolicy, AsynchronousBackoffPolicy>();
            container.Register<IWaitHandle, ManualResetEventWaitHandle>();
            container.Register<IApplicationResourceSettingNameProvider, ApplicationResourceSettingNameProvider>();
            container.Register<IApplicationResourceFactory, ApplicationResourceFactory>();
            container.Register<IApplicationResourceSettingProvider, ApplicationResourceSettingProvider>();
            container.Register<ILoggerFactory, LoggerFactory>();
            container.Register<IComponentHost, ComponentHost>();
            container.Register<IEmailManager, EmailManager>();

            if (loggerExtension == null)
            {
                container.Register<ILoggerExtension, NullLoggerExtension>();
            }
            else
            {
                container.Register(typeof(ILoggerExtension), loggerExtension);
            }
            container.RegisterInstance<IComponentFactory>(new ComponentFactory(container));
        }
    }
}
