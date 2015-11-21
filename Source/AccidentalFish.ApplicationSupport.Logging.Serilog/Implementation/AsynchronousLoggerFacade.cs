using System;
using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using ISerilogLogger = Serilog.ILogger;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog.Implementation
{
    internal class AsynchronousLoggerFacade : IAsynchronousLogger<ISerilogLogger>, ISerilogLogger
    {
        private readonly ISerilogLogger _logger;

        public AsynchronousLoggerFacade(ISerilogLogger logger)
        {
            _logger = logger;
        }

        public Task VerboseAsync(string message, params object[] additionalData)
        {
            _logger.Verbose(message, additionalData);
            return Task.FromResult(0);
        }

        public Task VerboseAsync(string message, Exception exception, params object[] additionalData)
        {
            _logger.Verbose(exception, message, additionalData);
            return Task.FromResult(0);
        }

        public Task DebugAsync(string message, params object[] additionalData)
        {
            _logger.Debug(message, additionalData);
            return Task.FromResult(0);
        }

        public Task DebugAsync(string message, Exception exception, params object[] additionalData)
        {
            _logger.Debug(exception, message, additionalData);
            return Task.FromResult(0);
        }

        public Task InformationAsync(string message, params object[] additionalData)
        {
            _logger.Information(message, additionalData);
            return Task.FromResult(0);
        }

        public Task InformationAsync(string message, Exception exception, params object[] additionalData)
        {
            _logger.Information(exception, message, additionalData);
            return Task.FromResult(0);
        }

        public Task WarningAsync(string message, params object[] additionalData)
        {
            _logger.Warning(message, additionalData);
            return Task.FromResult(0);
        }

        public Task WarningAsync(string message, Exception exception, params object[] additionalData)
        {
            _logger.Warning(exception, message, additionalData);
            return Task.FromResult(0);
        }

        public Task ErrorAsync(string message, params object[] additionalData)
        {
            _logger.Error(message, additionalData);
            return Task.FromResult(0);
        }

        public Task ErrorAsync(string message, Exception exception, params object[] additionalData)
        {
            _logger.Error(exception, message, additionalData);
            return Task.FromResult(0);
        }

        public Task FatalAsync(string message, params object[] additionalData)
        {
            _logger.Fatal(message, additionalData);
            return Task.FromResult(0);
        }

        public Task FatalAsync(string message, Exception exception, params object[] additionalData)
        {
            _logger.Fatal(exception, message, additionalData);
            return Task.FromResult(0);
        }

        public Task LogAsync(LogLevelEnum level, string message, params object[] additionalData)
        {
            _logger.Write(level.ToLogEventLevel(), message, additionalData);
            return Task.FromResult(0);
        }

        public Task LogAsync(LogLevelEnum level, string message, Exception exception, params object[] additionalData)
        {
            _logger.Write(level.ToLogEventLevel(), exception, message, additionalData);
            return Task.FromResult(0);
        }

        ISerilogLogger ISerilogLogger.ForContext(IEnumerable<ILogEventEnricher> enrichers)
        {
            return _logger.ForContext(enrichers);
        }

        ISerilogLogger ISerilogLogger.ForContext(string propertyName, object value, bool destructureObjects)
        {
            return _logger.ForContext(propertyName, value, destructureObjects);
        }

        ISerilogLogger ISerilogLogger.ForContext<TSource>()
        {
            return _logger.ForContext<TSource>();
        }

        ISerilogLogger ISerilogLogger.ForContext(Type source)
        {
            return _logger.ForContext(source);
        }

        void ISerilogLogger.Write(LogEvent logEvent)
        {
            _logger.Write(logEvent);
        }

        void ISerilogLogger.Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        {
            _logger.Write(level, messageTemplate, propertyValues);
        }

        void ISerilogLogger.Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Write(level, exception, messageTemplate, propertyValues);
        }

        bool ISerilogLogger.IsEnabled(LogEventLevel level)
        {
            return _logger.IsEnabled(level);
        }

        void ISerilogLogger.Verbose(string messageTemplate, params object[] propertyValues)
        {
            _logger.Verbose(messageTemplate, propertyValues);
        }

        void ISerilogLogger.Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Verbose(exception, messageTemplate, propertyValues);
        }

        void ISerilogLogger.Debug(string messageTemplate, params object[] propertyValues)
        {
            _logger.Debug(messageTemplate, propertyValues);
        }

        void ISerilogLogger.Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Debug(exception, messageTemplate, propertyValues);
        }

        void ISerilogLogger.Information(string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(messageTemplate, propertyValues);
        }

        void ISerilogLogger.Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(exception, messageTemplate, propertyValues);
        }

        void ISerilogLogger.Warning(string messageTemplate, params object[] propertyValues)
        {
            _logger.Warning(messageTemplate, propertyValues);
        }

        void ISerilogLogger.Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Warning(exception, messageTemplate, propertyValues);
        }

        void ISerilogLogger.Error(string messageTemplate, params object[] propertyValues)
        {
            _logger.Error(messageTemplate, propertyValues);
        }

        void ISerilogLogger.Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Error(exception, messageTemplate, propertyValues);
        }

        void ISerilogLogger.Fatal(string messageTemplate, params object[] propertyValues)
        {
            _logger.Fatal(messageTemplate, propertyValues);
        }

        void ISerilogLogger.Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Fatal(exception, messageTemplate, propertyValues);
        }

        public ISerilogLogger ActualLogger => _logger;
    }
}
