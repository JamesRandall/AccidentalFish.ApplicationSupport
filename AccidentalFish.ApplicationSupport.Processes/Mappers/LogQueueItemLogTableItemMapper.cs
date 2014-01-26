using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Mappers;

namespace AccidentalFish.ApplicationSupport.Processes.Mappers
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
                StackTrace = @from.StackTrace,
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
                StackTrace = @from.StackTrace,
                Source = @from.Source
            };
        }
    }
}
