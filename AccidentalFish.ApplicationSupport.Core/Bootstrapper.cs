using System;
using AccidentalFish.ApplicationSupport.Core.BackgroundProcesses;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Components.Implementation;
using AccidentalFish.ApplicationSupport.Core.Email;
using AccidentalFish.ApplicationSupport.Core.Email.Providers;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Implementation;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Policies.Implementation;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Core.Repository.Implementaton;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Core.Runtime.Implementation;
using AccidentalFish.ApplicationSupport.Core.Threading;
using AccidentalFish.ApplicationSupport.Core.Threading.Implementation;
using Microsoft.Practices.Unity;

namespace AccidentalFish.ApplicationSupport.Core
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IUnityContainer container)
        {
            RegisterDependencies(container, EmailProviderEnum.SendGrid);
        }

        public static void RegisterDependencies(
            IUnityContainer container,
            EmailProviderEnum emailProvider)
        {
            if (emailProvider == EmailProviderEnum.AmazonSimpleEmailService)
            {
                container.RegisterType<IEmailProvider, AmazonSimpleEmailProvider>();
            }
            else if (emailProvider == EmailProviderEnum.SendGrid)
            {
                container.RegisterType<IEmailProvider, SendGridEmailProvider>();
            }
            else
            {
                throw new NotImplementedException("Email provider not recognised");
            }

            if (!container.IsRegistered<ISqlRetryPolicy>())
            {
                container.RegisterType<ISqlRetryPolicy, NullSqlRetryPolicy>();
            }
            container.RegisterType<IUnitOfWorkFactory, EntityFrameworkUnitOfWorkFactory>();
            container.RegisterType<IBackoffPolicy, BackoffPolicy>();
            container.RegisterType<IAsynchronousBackoffPolicy, AsynchronousBackoffPolicy>();
            container.RegisterType<IWaitHandle, ManualResetEventWaitHandle>();
            container.RegisterType<IApplicationResourceSettingNameProvider, ApplicationResourceSettingNameProvider>();
            container.RegisterType<IApplicationResourceFactory, ApplicationResourceFactory>();
            container.RegisterType<IApplicationResourceSettingProvider, ApplicationResourceSettingProvider>();
            container.RegisterType<ILoggerFactory, LoggerFactory>();
            container.RegisterType<IComponentHost, ComponentHost>();

            container.RegisterInstance(container);
        }

        public static void RegisterInfrastructure(IUnityContainer container)
        {
            container.RegisterType<IHostableComponent, LogQueueProcessor>(HostableComponentNames.LogQueueProcessor);
        }
    }
}
