using AccidentalFish.ApplicationSupport.Core.Mappers;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Processes.Mappers
{
    internal class MapperFactory : IMapperFactory
    {
        public IMapper<LogQueueItem, LogTableItem> GetLogQueueItemLogTableItemMapper()
        {
            return new LogQueueItemLogTableItemMapper();
        }
    }
}
