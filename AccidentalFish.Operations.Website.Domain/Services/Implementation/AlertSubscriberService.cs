using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Components;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.Operations.Website.Domain.Mappers;
using AccidentalFish.Operations.Website.Domain.ViewModel;
using AlertSubscriber = AccidentalFish.ApplicationSupport.Azure.Alerts.Model.AlertSubscriber;

namespace AccidentalFish.Operations.Website.Domain.Services.Implementation
{
    internal class AlertSubscriberService : IAlertSubscriberService
    {
        private readonly IMapperFactory _mapperFactory;
        private readonly IAsynchronousTableStorageRepository<AlertSubscriber> _subscriberTable;

        public AlertSubscriberService(IAzureApplicationResourceFactory applicationResourceFactory,
            IMapperFactory mapperFactory)
        {
            _mapperFactory = mapperFactory;
            _subscriberTable = applicationResourceFactory.GetTableStorageRepository<AlertSubscriber>(new ComponentIdentity("com.accidentalfish.alert-sender"));
        }

        public async Task<PageResult<ViewModel.AlertSubscriber>> GetSubscribers(int page, int pageSize)
        {
            // this is insanely niave but is "ok" for now as I'm only expecting a handful of subscribers
            List<AlertSubscriber> subscribers = new List<AlertSubscriber>();
            await _subscriberTable.AllActionAsync(subscribers.AddRange);
            subscribers = subscribers.OrderBy(x => x.Email).ToList();
            IEnumerable<AlertSubscriber> slice = subscribers.Skip(page*pageSize).Take(pageSize).ToList();
            return new PageResult<ViewModel.AlertSubscriber>
            {
                Page = _mapperFactory.GetAlertSubscriberMapper().Map(slice),
                TotalRows = subscribers.Count
            };
        }

        public async Task Create(ViewModel.AlertSubscriber model)
        {
            AlertSubscriber subscriber = _mapperFactory.GetAlertSubscriberMapper().Map(model);
            subscriber.SetPartitionAndRowKey();
            await _subscriberTable.InsertAsync(subscriber);
        }
    }
}
