using AccidentalFish.ApplicationSupport.Azure.Alerts.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.Operations.Website.Domain.Mappers.Implementation
{
    internal class MapperFactory : IMapperFactory
    {
        public IBidirectionalMapper<AlertSubscriber, ViewModel.AlertSubscriber> GetAlertSubscriberMapper()
        {
            return new AlertSubscriberMapper();
        }
    }
}
