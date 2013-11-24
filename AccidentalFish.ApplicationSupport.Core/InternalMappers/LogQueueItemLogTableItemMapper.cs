using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.ApplicationSupport.Core.InternalMappers
{
    internal class LogQueueItemLogTableItemMapper : AbstractMapper<LogQueueItem, LogTableItem>
    {
        public override LogTableItem Map(LogQueueItem @from)
        {
            return new LogTableItem
            {
                ExceptionName = @from.ExceptionName,
                InnerExceptionName = @from.InnerExceptionName,
                Level = (int) @from.Level,
                LoggedAt = @from.LoggedAt,
                Message = @from.Message,
                RoleIdentifier = @from.RoleIdentifier,
                RoleName = @from.RoleName,
                Source = @from.Source
            };
        }

        public override LogQueueItem Map(LogTableItem @from)
        {
            return new LogQueueItem
            {
                ExceptionName = @from.ExceptionName,
                InnerExceptionName = @from.InnerExceptionName,
                Level = (LogLevelEnum)@from.Level,
                LoggedAt = @from.LoggedAt,
                Message = @from.Message,
                RoleIdentifier = @from.RoleIdentifier,
                RoleName = @from.RoleName,
                Source = @from.Source
            };
        }
    }
}
