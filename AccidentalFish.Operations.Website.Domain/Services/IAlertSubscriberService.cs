using System.Threading.Tasks;
using AccidentalFish.Operations.Website.Domain.ViewModel;

namespace AccidentalFish.Operations.Website.Domain.Services
{
    public interface IAlertSubscriberService
    {
        Task<PageResult<AlertSubscriber>> GetSubscribers(int page, int pageSize);
        Task Create(AlertSubscriber model);
    }
}
