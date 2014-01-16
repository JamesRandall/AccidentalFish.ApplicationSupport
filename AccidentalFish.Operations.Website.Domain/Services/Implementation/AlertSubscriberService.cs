using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Alerts.Model;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.NoSql;

namespace AccidentalFish.Operations.Website.Domain.Services.Implementation
{
    internal class AlertSubscriberService : IAlertSubscriberService
    {
        private readonly IAsynchronousNoSqlRepository<AlertSubscriber> _subscriberTable;

        public AlertSubscriberService(IApplicationResourceFactory applicationResourceFactory)
        {
            _subscriberTable = applicationResourceFactory.GetNoSqlRepository<AlertSubscriber>(new ComponentIdentity("com.accidentalfish.alert-sender"));
        }

        public Task<IEnumerable<ViewModel.AlertSubscriber>> GetSubscribers(int page, int pageSize, string sortColumn, string sortOrder)
        {
            return null;
        }
    }
}
