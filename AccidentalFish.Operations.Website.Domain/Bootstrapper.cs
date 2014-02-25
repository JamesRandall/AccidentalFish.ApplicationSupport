using AccidentalFish.Operations.Website.Domain.Mappers;
using AccidentalFish.Operations.Website.Domain.Mappers.Implementation;
using AccidentalFish.Operations.Website.Domain.Services;
using AccidentalFish.Operations.Website.Domain.Services.Implementation;
using Microsoft.Practices.Unity;

namespace AccidentalFish.Operations.Website.Domain
{
    public static class Bootstrapper
    {
        public static void RegisterDependencies(IUnityContainer container)
        {
            container.RegisterType<IMapperFactory, MapperFactory>();
            container.RegisterType<IAlertSubscriberService, AlertSubscriberService>();
            container.RegisterType<ILogViewerService, LogViewerService>();
        }
    }
}
