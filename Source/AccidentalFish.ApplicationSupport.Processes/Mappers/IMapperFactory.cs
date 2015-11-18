using AccidentalFish.ApplicationSupport.Core.Mappers;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Processes.Mappers
{
    internal interface IMapperFactory
    {
        IMapper<LogQueueItem, LogTableItem> GetLogQueueItemLogTableItemMapper();
    }
}
