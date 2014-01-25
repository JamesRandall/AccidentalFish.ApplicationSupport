using System;
using System.Diagnostics;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using CuttingEdge.Conditions;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class Logger : ILogger
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IAsynchronousQueue<LogQueueItem> _queue;
        private readonly IFullyQualifiedName _source;
        private readonly ILoggerExtension _loggerExtension;
        private readonly LogLevelEnum _minimumLoggingLevel;

        public Logger(
            IRuntimeEnvironment runtimeEnvironment,
            IAsynchronousQueue<LogQueueItem> queue,
            IFullyQualifiedName source,
            ILoggerExtension loggerExtension,
            LogLevelEnum minimumLoggingLevel)
        {
            Condition.Requires(queue).IsNotNull();
            Condition.Requires(source).IsNotNull();
            _runtimeEnvironment = runtimeEnvironment;
            _queue = queue;
            _source = source;
            _loggerExtension = loggerExtension;
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
            LogQueueItem item = CreateLogQueueItem(level, message, exception);
            _loggerExtension.Logger(item, level >= _minimumLoggingLevel);
            if (level >= _minimumLoggingLevel)
            {
                try
                {
                    await _queue.EnqueueAsync(item);
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
                RoleIdentifier = _runtimeEnvironment.RoleIdentifier,
                RoleName = _runtimeEnvironment.RoleName,
                Source = _source.FullyQualifiedName,
                StackTrace = exception != null ? exception.StackTrace : null
            };
        }
    }
}
