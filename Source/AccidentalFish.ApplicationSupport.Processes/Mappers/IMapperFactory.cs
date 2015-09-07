using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;
using AccidentalFish.ApplicationSupport.Processes.Logging.Model;

namespace AccidentalFish.ApplicationSupport.Processes.Mappers
{
    internal interface IMapperFactory
    {
        IMapper<LogQueueItem, LogTableItem> GetLogQueueItemLogTableItemMapper();
    }
}
