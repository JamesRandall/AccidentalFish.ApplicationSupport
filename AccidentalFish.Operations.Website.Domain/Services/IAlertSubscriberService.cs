using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.Operations.Website.Domain.Services
{
    public interface IAlertSubscriberService
    {
        Task<IEnumerable<ViewModel.AlertSubscriber>> GetSubscribers(int page, int pageSize, string sortColumn, string sortOrder);
    }
}
