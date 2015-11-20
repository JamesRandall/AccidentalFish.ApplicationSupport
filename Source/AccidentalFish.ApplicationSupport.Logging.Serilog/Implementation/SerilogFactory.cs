using System;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using Serilog;
using Serilog.Events;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog.Implementation
{
    internal class SerilogFactory : ISerilogFactory
    {
        private readonly Func<LoggerConfiguration> _loggerConfigurationProvider;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly LogLevelEnum _defaultMinimumLogLevel;
        private readonly string _sourceFqnPropertyName;
        private readonly string _correlationIdPropertyName;

        public SerilogFactory(Func<LoggerConfiguration> loggerConfigurationProvider,
            ICorrelationIdProvider correlationIdProvider,
            LogLevelEnum defaultMinimumLogLevel,
            string sourceFqnPropertyName,
            string correlationIdPropertyName)
        {
            _loggerConfigurationProvider = loggerConfigurationProvider;
            _correlationIdProvider = correlationIdProvider;
            _defaultMinimumLogLevel = defaultMinimumLogLevel;
            _sourceFqnPropertyName = sourceFqnPropertyName;
            _correlationIdPropertyName = correlationIdPropertyName;
        }

        public IAsynchronousLogger CreateAsynchronousLogger(LogLevelEnum? minimumLogLevel = null)
        {
            throw new NotImplementedException("SeriLog only supports synchronous logging, use CreateLogger().");
        }

        public Core.Logging.ILogger CreateLogger(LogLevelEnum? minimumLogLevel = null)
        {
            LoggerConfiguration loggerConfiguration = GetLoggerConfiguration(minimumLogLevel);
            return new LoggerFacade(loggerConfiguration.CreateLogger());
        }

        public IAsynchronousLogger CreateAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            throw new NotImplementedException("SeriLog only supports synchronous logging, use CreateLogger().");
        }

        public global::Serilog.ILogger CreateSerilog(LogLevelEnum? minimumLogLevel = null)
        {
            LoggerConfiguration loggerConfiguration = GetLoggerConfiguration(minimumLogLevel);
            return new LoggerFacade(loggerConfiguration.CreateLogger());
        }

        public Core.Logging.ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            LoggerConfiguration loggerConfiguration = GetLoggerConfiguration(minimumLogLevel);
            loggerConfiguration.Enrich.With(new FullyQualifiedNameEnricher(source, _sourceFqnPropertyName));
            return new LoggerFacade(loggerConfiguration.CreateLogger());
        }

        public global::Serilog.ILogger CreateSerilog(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel = null)
        {
            LoggerConfiguration loggerConfiguration = GetLoggerConfiguration(minimumLogLevel);
            return new LoggerFacade(loggerConfiguration.CreateLogger());
        }

        public global::Serilog.ILogger CreateSerilog(LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration.CreateLogger();
        }

        private LoggerConfiguration GetLoggerConfiguration(LogLevelEnum? minimumLogLevel)
        {
            var configuration = _loggerConfigurationProvider != null ?
                _loggerConfigurationProvider() :
                new LoggerConfiguration().WriteTo.Trace();
            configuration
                .MinimumLevel
                .Is(minimumLogLevel.GetValueOrDefault(_defaultMinimumLogLevel).ToLogEventLevel())
                .Enrich.With(new CorrelationIdEnricher(_correlationIdProvider, _correlationIdPropertyName));
            return configuration;
        }
    }
}
