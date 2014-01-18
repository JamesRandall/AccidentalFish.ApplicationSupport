using AccidentalFish.ApplicationSupport.Core.Alerts.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.Operations.Website.Domain.Mappers.Implementation
{
    internal class MapperFactory : IMapperFactory
    {
        public IMapper<AlertSubscriber, ViewModel.AlertSubscriber> GetAlertSubscriberMapper()
        {
            return new AlertSubscriberMapper();
        }
    }
}
