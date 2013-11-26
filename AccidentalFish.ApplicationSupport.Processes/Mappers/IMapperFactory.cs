using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.ApplicationSupport.Processes.Mappers
{
    internal interface IMapperFactory
    {
        IMapper<LogQueueItem, LogTableItem> GetLogQueueItemLogTableItemMapper();
    }
}
