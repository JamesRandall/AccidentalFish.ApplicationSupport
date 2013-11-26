using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Processes.Logging;
using AccidentalFish.ApplicationSupport.Processes.Mappers;
using Microsoft.Practices.Unity;

namespace AccidentalFish.ApplicationSupport.Processes
{
    public static class Bootstrapper
    {
        public static void RegistgerDependencies(IUnityContainer container)
        {
            container.RegisterType<IMapperFactory, MapperFactory>();
            container.RegisterType<IHostableComponent, LogQueueProcessor>(HostableComponentNames.LogQueueProcessor);
        }
    }
}
