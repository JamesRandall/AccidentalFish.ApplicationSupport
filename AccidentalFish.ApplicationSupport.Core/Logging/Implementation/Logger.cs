using System;
using System.Diagnostics;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using CuttingEdge.Conditions;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class Logger : ILogger
    {
        private readonly IAsynchronousQueue<LogQueueItem> _queue;
        private readonly IFullyQualifiedName _source;
        private readonly LogLevelEnum _minimumLoggingLevel;

        public Logger(IAsynchronousQueue<LogQueueItem> queue, IFullyQualifiedName source, LogLevelEnum minimumLoggingLevel)
        {
            Condition.Requires(queue).IsNotNull();
            Condition.Requires(source).IsNotNull();
            _queue = queue;
            _source = source;
            _minimumLoggingLevel = minimumLoggingLevel;
        }

        public void Debug(string message)
        {
            Log(LogLevelEnum.Debug, message);
        }

        public void Debug(string message, Exception exception)
        {
            Log(LogLevelEnum.Debug, message, exception);
        }

        public void Information(string message)
        {
            Log(LogLevelEnum.Information, message);
        }

        public void Information(string message, Exception exception)
        {
            Log(LogLevelEnum.Information, message, exception);
        }

        public void Warning(string message)
        {
            Log(LogLevelEnum.Warning, message);
        }

        public void Warning(string message, Exception exception)
        {
            Log(LogLevelEnum.Warning, message, exception);
        }

        public void Error(string message)
        {
            Log(LogLevelEnum.Error, message);
        }

        public void Error(string message, Exception exception)
        {
            Log(LogLevelEnum.Error, message, exception);
        }

        public void Log(LogLevelEnum level, string message)
        {
            Log(level, message, null);
        }

        public async void Log(LogLevelEnum level, string message, Exception exception)
        {
            if (level >= _minimumLoggingLevel)
            {
                try
                {
                    await _queue.EnqueueAsync(CreateLogQueueItem(level, message, exception));
                }
                catch (Exception)
                {
                    Trace.TraceError("Unable to enqueue log queue item");
                }
            }
        }

        private LogQueueItem CreateLogQueueItem(LogLevelEnum level, string message, Exception exception)
        {
            return new LogQueueItem
            {
                ExceptionName = exception != null ? exception.GetType().FullName : null,
                InnerExceptionName = exception != null && exception.InnerException != null ? exception.InnerException.GetType().FullName : null,
                Level = level,
                LoggedAt = DateTimeOffset.UtcNow,
                Message = message,
                Source = _source.FullyQualifiedName,
                StackTrace = exception != null ? exception.StackTrace : null
            };
        }
    }
}
