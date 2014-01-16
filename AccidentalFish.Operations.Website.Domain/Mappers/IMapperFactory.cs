using AccidentalFish.ApplicationSupport.Core.Alerts.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.Operations.Website.Domain.Mappers
{
    public interface IMapperFactory
    {
        IMapper<AlertSubscriber, ViewModel.AlertSubscriber> GetAlertSubscriberMapper();
    }
}
