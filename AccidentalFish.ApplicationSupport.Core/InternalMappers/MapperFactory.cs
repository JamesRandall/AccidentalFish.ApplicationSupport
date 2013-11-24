using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.ApplicationSupport.Core.InternalMappers
{
    internal class MapperFactory : IMapperFactory
    {
        public IMapper<LogQueueItem, LogTableItem> GetLogQueueItemLogTableItemMapper()
        {
            return new LogQueueItemLogTableItemMapper();
        }
    }
}
