using AccidentalFish.ApplicationSupport.Azure.Alerts.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.Operations.Website.Domain.Mappers.Implementation
{
    internal class AlertSubscriberMapper : AbstractBidirectionalMapper<AlertSubscriber, ViewModel.AlertSubscriber>
    {
        public override ViewModel.AlertSubscriber Map(AlertSubscriber @from)
        {
            return new ViewModel.AlertSubscriber
            {
                Email = @from.Email,
                Mobile = @from.Mobile
            };
        }

        public override AlertSubscriber Map(ViewModel.AlertSubscriber @from)
        {
            return new AlertSubscriber
            {
                Email = @from.Email,
                Mobile = @from.Mobile
            };
        }
    }
}
