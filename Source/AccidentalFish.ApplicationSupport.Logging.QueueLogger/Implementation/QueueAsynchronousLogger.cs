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
    internal class QueueAsynchronousLogger : IAsynchronousLogger<IAsynchronousQueue<LogQueueItem>>
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly IAsynchronousQueue<LogQueueItem> _queue;
        private readonly IFullyQualifiedName _source;
        private readonly IQueueLoggerExtension _queueLoggerExtension;
        private readonly LogLevelEnum _minimumLoggingLevel;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public QueueAsynchronousLogger(
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

        public async Task VerboseAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Verbose, message, additionalData);
        }

        public async Task VerboseAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Verbose, message, exception, additionalData);
        }

        public async Task DebugAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Debug, message, additionalData);
        }

        public async Task DebugAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Debug, message, exception, additionalData);
        }

        public async Task InformationAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Information, message, additionalData);
        }

        public async Task InformationAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Information, message, exception, additionalData);
        }

        public async Task WarningAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Warning, message, additionalData);
        }

        public async Task WarningAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Warning, message, exception, additionalData);
        }

        public async Task ErrorAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Error, message, additionalData);
        }

        public async Task ErrorAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Error, message, exception, additionalData);
        }

        public async Task FatalAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Fatal, message, additionalData);
        }

        public async Task FatalAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Fatal, message, exception, additionalData);
        }

        public async Task LogAsync(LogLevelEnum level, string message, params object[] additionalData)
        {
            await LogAsync(level, message, null, additionalData);
        }

        public async Task LogAsync(LogLevelEnum level, string message, Exception exception, params object[] additionalData)
        {
            LogQueueItem item = CreateLogQueueItem(level, message, exception, additionalData);
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

        private LogQueueItem CreateLogQueueItem(LogLevelEnum level, string message, Exception exception, params object[] additionalData)
        {
            return new LogQueueItem
            {
                CorrelationId = _correlationIdProvider.CorrelationId,
                ExceptionName = exception?.GetType().FullName,
                InnerExceptionName = exception?.InnerException?.GetType().FullName,
                Level = level,
                LoggedAt = DateTimeOffset.UtcNow,
                Message = string.Format(message,additionalData),
                RoleIdentifier = _runtimeEnvironment.RoleIdentifier,
                RoleName = _runtimeEnvironment.RoleName,
                Source = _source.FullyQualifiedName,
                StackTrace = exception?.StackTrace
            };
        }

        public IAsynchronousQueue<LogQueueItem> ActualLogger => _queue;
    }
}
