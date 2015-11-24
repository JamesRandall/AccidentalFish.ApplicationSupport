using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Extensions;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation
{
    internal class QueueAsynchronousLogger : IAsynchronousLogger<CloudQueue>
    {
        private readonly IRuntimeEnvironment _runtimeEnvironment;
        private readonly CloudQueue _queue;
        private readonly IQueueSerializer _queueSerializer;
        private readonly IFullyQualifiedName _source;
        private readonly IQueueLoggerExtension _queueLoggerExtension;
        private readonly LogLevelEnum _minimumLoggingLevel;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public QueueAsynchronousLogger(
            IRuntimeEnvironment runtimeEnvironment,
            CloudQueue queue,
            IQueueSerializer queueSerializer,
            IFullyQualifiedName source,
            IQueueLoggerExtension queueLoggerExtension,
            LogLevelEnum minimumLoggingLevel,
            ICorrelationIdProvider correlationIdProvider)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _queue = queue;
            _queueSerializer = queueSerializer;
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
            // because the queue logger uses internal resources this prevents it eating itself
            if (_source != null && _source.IsFrameworkSource()) return;
            LogQueueItem item = CreateLogQueueItem(level, message, exception, additionalData);
            bool willLog = level >= _minimumLoggingLevel;
            if (_queueLoggerExtension.BeforeLog(item, exception, willLog))
            {
                if (willLog)
                {
                    try
                    {
                        CloudQueueMessage queueMessage = new CloudQueueMessage(_queueSerializer.Serialize(item));
                        await _queue.AddMessageAsync(queueMessage);
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
                Source = _source?.FullyQualifiedName,
                StackTrace = exception?.StackTrace
            };
        }

        public CloudQueue ActualLogger => _queue;
    }
}
