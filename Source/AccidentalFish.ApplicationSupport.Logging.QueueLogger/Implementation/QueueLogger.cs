using System;
using System.Diagnostics;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation
{
    internal class QueueLogger : ILogger<IQueue<LogQueueItem>>
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IQueue<LogQueueItem> _queue;
        private readonly IFullyQualifiedName _source;
        private readonly IQueueLoggerExtension _queueLoggerExtension;
        private readonly LogLevelEnum _minimumLoggingLevel;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public QueueLogger(
            IRuntimeEnvironment runtimeEnvironment,
            IQueue<LogQueueItem> queue,
            IFullyQualifiedName source,
            IQueueLoggerExtension queueLoggerExtension,
            LogLevelEnum minimumLoggingLevel,
            ICorrelationIdProvider correlationIdProvider)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _queue = queue;
            _source = source;
            _queueLoggerExtension = queueLoggerExtension;
            _minimumLoggingLevel = minimumLoggingLevel;
            _correlationIdProvider = correlationIdProvider;
        }

        public void Verbose(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Verbose, message, additionalData);
        }

        public void Verbose(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Verbose, message, exception, additionalData);
        }

        public void Debug(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Debug, message, additionalData);
        }

        public void Debug(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Debug, message, exception, additionalData);
        }

        public void Information(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Information, message, additionalData);
        }

        public void Information(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Information, message, exception, additionalData);
        }

        public void Warning(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Warning, message, additionalData);
        }

        public void Warning(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Warning, message, exception, additionalData);
        }

        public void Error(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Error, message, additionalData);
        }

        public void Error(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Error, message, exception, additionalData);
        }

        public void Fatal(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Fatal, message, additionalData);
        }

        public void Fatal(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Fatal, message, exception, additionalData);
        }

        public void Log(LogLevelEnum level, string message, params object[] additionalData)
        {
            Log(level, message, null, additionalData, additionalData);
        }

        public void Log(LogLevelEnum level, string message, Exception exception, params object[] additionalData)
        {
            LogQueueItem item = CreateLogQueueItem(level, message, exception, additionalData);
            bool willLog = level >= _minimumLoggingLevel;
            if (_queueLoggerExtension.BeforeLog(item, exception, willLog))
            {
                if (willLog)
                {
                    try
                    {
                        _queue.Enqueue(item);
                    }
                    catch (Exception)
                    {
                        Trace.TraceError("Unable to enqueue log queue item");
                    }
                }
            }
        }

        private LogQueueItem CreateLogQueueItem(LogLevelEnum level, string message, Exception exception, object[] additionalData)
        {
            return new LogQueueItem
            {
                CorrelationId = _correlationIdProvider.CorrelationId,
                ExceptionName = exception?.GetType().FullName,
                InnerExceptionName = exception?.InnerException?.GetType().FullName,
                Level = level,
                LoggedAt = DateTimeOffset.UtcNow,
                Message = string.Format(message, additionalData),
                RoleIdentifier = _runtimeEnvironment.RoleIdentifier,
                RoleName = _runtimeEnvironment.RoleName,
                Source = _source?.FullyQualifiedName,
                StackTrace = exception?.StackTrace
            };
        }

        public IQueue<LogQueueItem> ActualLogger => _queue;
    }
}
