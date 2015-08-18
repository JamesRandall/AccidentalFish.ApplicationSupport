using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;

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
            _runtimeEnvironment = runtimeEnvironment;
            _queue = queue;
            _source = source;
            _loggerExtension = loggerExtension;
            _minimumLoggingLevel = minimumLoggingLevel;
        }

        public async Task Debug(string message)
        {
            await Log(LogLevelEnum.Debug, message);
        }

        public async Task Debug(string message, Exception exception)
        {
            await Log(LogLevelEnum.Debug, message, exception);
        }

        public async Task Information(string message)
        {
            await Log(LogLevelEnum.Information, message);
        }

        public async Task Information(string message, Exception exception)
        {
            await Log(LogLevelEnum.Information, message, exception);
        }

        public async Task Warning(string message)
        {
            await Log(LogLevelEnum.Warning, message);
        }

        public async Task Warning(string message, Exception exception)
        {
            await Log(LogLevelEnum.Warning, message, exception);
        }

        public async Task Error(string message)
        {
            await Log(LogLevelEnum.Error, message);
        }

        public async Task Error(string message, Exception exception)
        {
            await Log(LogLevelEnum.Error, message, exception);
        }

        public async Task Log(LogLevelEnum level, string message)
        {
            await Log(level, message, null);
        }

        public async Task Log(LogLevelEnum level, string message, Exception exception)
        {
            LogQueueItem item = CreateLogQueueItem(level, message, exception);
            _loggerExtension.Logger(item, exception, level >= _minimumLoggingLevel);
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
