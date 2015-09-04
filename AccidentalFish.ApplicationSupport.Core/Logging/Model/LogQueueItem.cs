using System;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Model
{
    public class LogQueueItem
    {
        public string CorrelationId { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string ExceptionName { get; set; }

        public string StackTrace { get; set; }

        public string InnerExceptionName { get; set; }

        public string RoleIdentifier { get; set; }

        public string RoleName { get; set; }

        public LogLevelEnum Level { get; set; }

        public DateTimeOffset LoggedAt { get; set; }
    }
}
