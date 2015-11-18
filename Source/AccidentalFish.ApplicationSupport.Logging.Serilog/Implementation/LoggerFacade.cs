using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task VerboseAsync(string message)
        {
            _logger.Verbose(message);
            return Task.FromResult(0);
        }

        public Task VerboseAsync(string message, Exception exception)
        {
            _logger.Verbose(exception, message);
            return Task.FromResult(0);
        }

        public Task DebugAsync(string message)
        {
            _logger.Debug(message);
            return Task.FromResult(0);
        }

        public Task DebugAsync(string message, Exception exception)
        {
            _logger.Debug(exception, message);
            return Task.FromResult(0);
        }

        public Task InformationAsync(string message)
        {
            _logger.Information(message);
            return Task.FromResult(0);
        }

        public Task InformationAsync(string message, Exception exception)
        {
            _logger.Information(exception, message);
            return Task.FromResult(0);
        }

        public Task WarningAsync(string message)
        {
            _logger.Warning(message);
            return Task.FromResult(0);
        }

        public Task WarningAsync(string message, Exception exception)
        {
            _logger.Warning(exception, message);
            return Task.FromResult(0);
        }

        public Task ErrorAsync(string message)
        {
            _logger.Error(message);
            return Task.FromResult(0);
        }

        public Task ErrorAsync(string message, Exception exception)
        {
            _logger.Error(exception, message);
            return Task.FromResult(0);
        }

        public Task FatalAsync(string message)
        {
            _logger.Fatal(message);
            return Task.FromResult(0);
        }

        public Task FatalAsync(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
            return Task.FromResult(0);
        }

        public Task LogAsync(LogLevelEnum level, string message)
        {
            throw new NotImplementedException();
        }

        public Task LogAsync(LogLevelEnum level, string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public ISerilogLogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
        {
            return _logger.ForContext(enrichers);
        }

        public ISerilogLogger ForContext(string propertyName, object value, bool destructureObjects = false)
        {
            return _logger.ForContext(propertyName, value, destructureObjects);
        }

        public ISerilogLogger ForContext<TSource>()
        {
            return _logger.ForContext<TSource>();
        }

        public ISerilogLogger ForContext(Type source)
        {
            return _logger.ForContext(source);
        }

        public void Write(LogEvent logEvent)
        {
            _logger.Write(logEvent);
        }

        public void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        {
            _logger.Write(level, messageTemplate, propertyValues);
        }

        public void Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Write(level, exception, messageTemplate, propertyValues);
        }

        public bool IsEnabled(LogEventLevel level)
        {
            return _logger.IsEnabled(level);
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            _logger.Verbose(messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Verbose(exception, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            _logger.Debug(messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Debug(exception, messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(exception, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            _logger.Warning(messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Warning(exception, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            _logger.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Error(exception, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            _logger.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Fatal(exception, messageTemplate, propertyValues);
        }

        public ISerilogLogger ActualLogger => _logger;
    }
}
