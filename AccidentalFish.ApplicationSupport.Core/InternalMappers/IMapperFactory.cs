using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.ApplicationSupport.Core.InternalMappers
{
    internal interface IMapperFactory
    {
        IMapper<LogQueueItem, LogTableItem> GetLogQueueItemLogTableItemMapper();
    }
}
