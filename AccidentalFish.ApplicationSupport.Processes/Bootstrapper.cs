using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Processes.Email;
using AccidentalFish.ApplicationSupport.Processes.Logging;
using AccidentalFish.ApplicationSupport.Processes.Mappers;

namespace AccidentalFish.ApplicationSupport.Processes
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IDependencyResolver resolver)
        {
            resolver.Register<IMapperFactory, MapperFactory>();
            resolver.Register<IHostableComponent, LogQueueProcessor>(HostableComponentNames.LogQueueProcessor);
            resolver.Register<IHostableComponent, EmailQueueProcessor>(HostableComponentNames.EmailQueueProcessor);
        }
    }
}
