using System;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Processes.Email;
using AccidentalFish.ApplicationSupport.Processes.Logging;
using AccidentalFish.ApplicationSupport.Processes.Mappers;

namespace AccidentalFish.ApplicationSupport.Processes
{
    public static class Bootstrapper
    {
        public static IDependencyResolver UseHostableProcesses(this IDependencyResolver resolver)
        {
            resolver.Register<IMapperFactory, MapperFactory>();
            resolver.Register<IHostableComponent, LogQueueProcessor>(HostableComponentNames.LogQueueProcessor);
            resolver.Register<IHostableComponent, EmailQueueProcessor>(HostableComponentNames.EmailQueueProcessor);
            resolver.Register<ITemplateEngineFactory, TemplateEngineFactory>();
            return resolver;
        }

        [Obsolete]
        public static void RegisterDependencies(IDependencyResolver resolver)
        {
            UseHostableProcesses(resolver);
        }
    }
}
