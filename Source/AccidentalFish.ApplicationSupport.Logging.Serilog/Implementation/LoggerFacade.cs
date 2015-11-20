using System;
using System.Collections.Generic;
using AccidentalFish.ApplicationSupport.Core.Logging;
using Serilog.Core;
using Serilog.Events;
using ISerilogLogger = Serilog.ILogger;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog.Implementation
{
    internal class LoggerFacade : ILogger<ISerilogLogger>, ISerilogLogger
    {
        private readonly ISerilogLogger _logger;

        public LoggerFacade(ISerilogLogger logger)
        {
            _logger = logger;
        }

        public void Verbose(string message)
        {
            _logger.Verbose(message);
        }

        public void Verbose(string message, Exception exception)
        {
            _logger.Verbose(exception, message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            _logger.Debug(exception, message);
        }

        public void Information(string message)
        {
            _logger.Information(message);
        }

        public void Information(string message, Exception exception)
        {
            _logger.Information(exception, message);
        }

        public void Warning(string message)
        {
            _logger.Warning(message);
        }

        public void Warning(string message, Exception exception)
        {
            _logger.Warning(exception, message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
        }

        public void Log(LogLevelEnum level, string message)
        {
            _logger.Write(level.ToLogEventLevel(), message);
        }

        public void Log(LogLevelEnum level, string message, Exception exception)
        {
            _logger.Write(level.ToLogEventLevel(), exception, message);
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
