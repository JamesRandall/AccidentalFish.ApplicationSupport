using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation
{
    internal class QueueLogger : ILogger
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IAsynchronousQueue<LogQueueItem> _queue;
        private readonly IFullyQualifiedName _source;
        private readonly IQueueLoggerExtension _queueLoggerExtension;
        private readonly LogLevelEnum _minimumLoggingLevel;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public QueueLogger(
            IRuntimeEnvironment runtimeEnvironment,
            IAsynchronousQueue<LogQueueItem> queue,
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

        public async Task VerboseAsync(string message)
        {
            await LogAsync(LogLevelEnum.Verbose, message);
        }

        public async Task VerboseAsync(string message, Exception exception)
        {
            await LogAsync(LogLevelEnum.Verbose, message, exception);
        }

        public async Task DebugAsync(string message)
        {
            await LogAsync(LogLevelEnum.Debug, message);
        }

        public async Task DebugAsync(string message, Exception exception)
        {
            await LogAsync(LogLevelEnum.Debug, message, exception);
        }

        public async Task InformationAsync(string message)
        {
            await LogAsync(LogLevelEnum.Information, message);
        }

        public async Task InformationAsync(string message, Exception exception)
        {
            await LogAsync(LogLevelEnum.Information, message, exception);
        }

        public async Task WarningAsync(string message)
        {
            await LogAsync(LogLevelEnum.Warning, message);
        }

        public async Task WarningAsync(string message, Exception exception)
        {
            await LogAsync(LogLevelEnum.Warning, message, exception);
        }

        public async Task ErrorAsync(string message)
        {
            await LogAsync(LogLevelEnum.Error, message);
        }

        public async Task ErrorAsync(string message, Exception exception)
        {
            await LogAsync(LogLevelEnum.Error, message, exception);
        }

        public async Task FatalAsync(string message)
        {
            await LogAsync(LogLevelEnum.Fatal, message);
        }

        public async Task FatalAsync(string message, Exception exception)
        {
            await LogAsync(LogLevelEnum.Fatal, message, exception);
        }

        public async Task LogAsync(LogLevelEnum level, string message)
        {
            await LogAsync(level, message, null);
        }

        public async Task LogAsync(LogLevelEnum level, string message, Exception exception)
        {
            LogQueueItem item = CreateLogQueueItem(level, message, exception);
            bool willLog = level >= _minimumLoggingLevel;
            if (_queueLoggerExtension.BeforeLog(item, exception, willLog))
            {
                if (willLog)
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
        }

        private LogQueueItem CreateLogQueueItem(LogLevelEnum level, string message, Exception exception)
        {
            return new LogQueueItem
            {
                CorrelationId = _correlationIdProvider.CorrelationId,
                ExceptionName = exception?.GetType().FullName,
                InnerExceptionName = exception?.InnerException?.GetType().FullName,
                Level = level,
                LoggedAt = DateTimeOffset.UtcNow,
                Message = message,
                RoleIdentifier = _runtimeEnvironment.RoleIdentifier,
                RoleName = _runtimeEnvironment.RoleName,
                Source = _source.FullyQualifiedName,
                StackTrace = exception?.StackTrace
            };
        }
    }
}
