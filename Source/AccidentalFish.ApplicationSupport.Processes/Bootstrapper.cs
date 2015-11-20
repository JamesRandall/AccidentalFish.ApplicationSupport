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
            ITemplateEngineFactory templateEngineFactory = new TemplateEngineFactory();
            return resolver
                .Register<IMapperFactory, MapperFactory>()
                .Register<IHostableComponent, LogQueueProcessor>(HostableComponentNames.LogQueueProcessor)
                .Register<IHostableComponent, EmailQueueProcessor>(HostableComponentNames.EmailQueueProcessor)
                .RegisterInstance(templateEngineFactory);
        }
    }
}
