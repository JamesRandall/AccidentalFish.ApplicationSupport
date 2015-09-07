using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;
using AccidentalFish.ApplicationSupport.Processes.Logging.Model;

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
