using System;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger.Model
{
    /// <summary>
    /// Log item as represented on a queue
    /// </summary>
    public class LogQueueItem
    {
        /// <summary>
        /// Optional correlation ID (used for tracking logs across mutliple services)
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// The source of the log item (typically a component identity)
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The message for the log item
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Name of the exception that triggered the logging, may be null.
        /// </summary>
        public string ExceptionName { get; set; }

        /// <summary>
        /// The stack track, if any, that triggered the exception
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Inner exception name, if available.
        /// </summary>
        public string InnerExceptionName { get; set; }

        /// <summary>
        /// Identity of the Azure worker role (or machine name)
        /// </summary>
        public string RoleIdentifier { get; set; }

        /// <summary>
        /// Name of the Azure worker role (or machine name)
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Log level
        /// </summary>
        public LogLevelEnum Level { get; set; }

        /// <summary>
        /// Date / time it was logged at
        /// </summary>
        public DateTimeOffset LoggedAt { get; set; }
    }
}
