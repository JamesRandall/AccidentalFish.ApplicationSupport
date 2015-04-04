using AccidentalFish.ApplicationSupport.Azure.Alerts.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.Operations.Website.Domain.Mappers
{
    public interface IMapperFactory
    {
        IBidirectionalMapper<AlertSubscriber, ViewModel.AlertSubscriber> GetAlertSubscriberMapper();
    }
}
