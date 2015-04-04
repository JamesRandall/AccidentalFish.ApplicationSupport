using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.Operations.Website.Domain.Model;

namespace AccidentalFish.Operations.Website.Domain.Services
{
    public interface ILogViewerService
    {
        Task<PagedResultSegment<LogTableItem>> GetByDateDescending(string continuationToken);
        Task<PagedResultSegment<LogTableItem>> GetBySeverity(string continuationToken);
        Task<PagedResultSegment<LogTableItem>> GetBySource(string continuationToken);
    }
}
