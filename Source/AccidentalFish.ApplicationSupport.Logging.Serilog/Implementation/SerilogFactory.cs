using System;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using Serilog;
using Serilog.Events;
using ILogger = AccidentalFish.ApplicationSupport.Core.Logging.ILogger;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog.Implementation
{
    internal class SerilogFactory : ISerilogFactory
    {
        private readonly Func<LoggerConfiguration> _loggerConfigurationProvider;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public SerilogFactory(Func<LoggerConfiguration> loggerConfigurationProvider, ICorrelationIdProvider correlationIdProvider)
        {
            _loggerConfigurationProvider = loggerConfigurationProvider;
            _correlationIdProvider = correlationIdProvider;
        }

        public ILogger CreateLogger(LogLevelEnum minimumLogLevel = LogLevelEnum.Warning)
        {
            LoggerConfiguration loggerConfiguration = GetLoggerConfiguration(minimumLogLevel);
            return new LoggerFacade(loggerConfiguration.CreateLogger());
        }

        public ILogger CreateLogger(IFullyQualifiedName source, LogLevelEnum minimumLogLevel = LogLevelEnum.Warning)
        {
            LoggerConfiguration loggerConfiguration = GetLoggerConfiguration(minimumLogLevel);
            loggerConfiguration.Enrich.With(new FullyQualifiedNameEnricher(source));
            return new LoggerFacade(loggerConfiguration.CreateLogger());
        }

        public global::Serilog.ILogger CreateSerilog(LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration.CreateLogger();
        }

        private LogEventLevel LogEventLevelFrom(LogLevelEnum logLevel)
        {
            switch (logLevel)
            {
                case LogLevelEnum.Verbose: return LogEventLevel.Verbose;
                case LogLevelEnum.Debug: return LogEventLevel.Debug;
                case LogLevelEnum.Information: return LogEventLevel.Information;
                case LogLevelEnum.Warning: return LogEventLevel.Warning;
                case LogLevelEnum.Error: return LogEventLevel.Error;
                case LogLevelEnum.Fatal: return LogEventLevel.Fatal;
            }
            throw new NotSupportedException($"Log level {logLevel} is not supported.");
        }

        private LoggerConfiguration GetLoggerConfiguration(LogLevelEnum minimumLogLevel)
        {
            var configuration = _loggerConfigurationProvider != null ?
                _loggerConfigurationProvider() :
                new LoggerConfiguration().WriteTo.Trace();
            configuration
                .MinimumLevel
                .Is(LogEventLevelFrom(minimumLogLevel))
                .Enrich.With(new CorrelationIdEnricher(_correlationIdProvider));

            return configuration;
        }
    }
}
