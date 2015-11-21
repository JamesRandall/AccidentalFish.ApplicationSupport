using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using Microsoft.ApplicationInsights;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights.Implementation
{
    internal class ApplicationInsightAsynchronousLogger : IAsynchronousLogger<TelemetryClient>
    {
        private readonly IFullyQualifiedName _fullyQualifiedName;
        private readonly LogLevelEnum _minimumLogLevel;
        private readonly TelemetryClient _telemetryClient = new TelemetryClient();

        public ApplicationInsightAsynchronousLogger(IFullyQualifiedName fullyQualifiedName, LogLevelEnum minimumLogLevel)
        {
            _fullyQualifiedName = fullyQualifiedName;
            _minimumLogLevel = minimumLogLevel;
        }

        public Task VerboseAsync(string message, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Verbose, message, additionalData);
        }

        public Task VerboseAsync(string message, Exception exception, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Verbose, message, exception, additionalData);
        }

        public Task DebugAsync(string message, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Debug, message, additionalData);
        }

        public Task DebugAsync(string message, Exception exception, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Debug, message, exception, additionalData);
        }

        public Task InformationAsync(string message, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Information, message, additionalData);
        }

        public Task InformationAsync(string message, Exception exception, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Information, message, exception, additionalData);
        }

        public Task WarningAsync(string message, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Warning, message);
        }

        public Task WarningAsync(string message, Exception exception, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Warning, message, exception, additionalData);
        }

        public Task ErrorAsync(string message, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Error, message, additionalData);
        }

        public Task ErrorAsync(string message, Exception exception, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Error, message, exception, additionalData);
        }

        public Task FatalAsync(string message, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Fatal, message, additionalData);
        }

        public Task FatalAsync(string message, Exception exception, params object[] additionalData)
        {
            return LogAsync(LogLevelEnum.Fatal, message, exception, additionalData);
        }

        public Task LogAsync(LogLevelEnum level, string message, params object[] additionalData)
        {
            return LogAsync(level, message, null, additionalData, additionalData);
        }

        public Task LogAsync(LogLevelEnum level, string message, Exception exception, params object[] additionalData)
        {
            if (level < _minimumLogLevel) return Task.FromResult(0);

            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                {"level", level.ToString()},
                {"message", string.Format(message, additionalData)}
            };
            if (_fullyQualifiedName != null)
            {
                properties.Add("component", _fullyQualifiedName.ToString());
            }

            if (exception != null)
            {
                _telemetryClient.TrackException(exception, properties);
            }
            _telemetryClient.TrackEvent($"LOG:{level}", properties);

            return Task.FromResult(0);
        }

        public TelemetryClient ActualLogger => _telemetryClient;
    }
}
