using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.NoSql;

namespace AccidentalFish.Operations.Website.Domain.Services
{
    public interface ILogViewerService
    {
        Task<PagedResultSegment<LogTableItem>> GetByDateDescending(string continuationToken);
        Task<PagedResultSegment<LogTableItem>> GetBySeverity(string continuationToken);
        Task<PagedResultSegment<LogTableItem>> GetBySource(string continuationToken);
    }
}
