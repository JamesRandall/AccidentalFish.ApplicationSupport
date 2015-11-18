using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights
{
    internal class ApplicationInsightLogger : ILogger
    {
        private readonly IFullyQualifiedName _fullyQualifiedName;
        private readonly LogLevelEnum _minimumLogLevel;
        private readonly Microsoft.ApplicationInsights.TelemetryClient _telemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();

        public ApplicationInsightLogger(IFullyQualifiedName fullyQualifiedName, LogLevelEnum minimumLogLevel)
        {
            _fullyQualifiedName = fullyQualifiedName;
            _minimumLogLevel = minimumLogLevel;
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

        public Task LogAsync(LogLevelEnum level, string message, Exception exception)
        {
            if (level < _minimumLogLevel) return Task.FromResult(0);

            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                {"level", level.ToString()},
                {"message", message}
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
    }
}
