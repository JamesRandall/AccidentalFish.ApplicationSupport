using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Naming;
using Microsoft.ApplicationInsights.DataContracts;

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

        public async Task DebugAsync(string message)
        {
            await Log(LogLevelEnum.Debug, message);
        }

        public async Task DebugAsync(string message, Exception exception)
        {
            await Log(LogLevelEnum.Debug, message, exception);
        }

        public async Task Information(string message)
        {
            await Log(LogLevelEnum.Information, message);
        }

        public async Task Information(string message, Exception exception)
        {
            await Log(LogLevelEnum.Information, message, exception);
        }

        public async Task Warning(string message)
        {
            await Log(LogLevelEnum.Warning, message);
        }

        public async Task Warning(string message, Exception exception)
        {
            await Log(LogLevelEnum.Warning, message, exception);
        }

        public async Task Error(string message)
        {
            await Log(LogLevelEnum.Error, message);
        }

        public async Task Error(string message, Exception exception)
        {
            await Log(LogLevelEnum.Error, message, exception);
        }

        public async Task Log(LogLevelEnum level, string message)
        {
            await Log(level, message, null);
        }

        public Task Log(LogLevelEnum level, string message, Exception exception)
        {
            if (level < _minimumLogLevel) return Task.FromResult(0);

            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                {"component", _fullyQualifiedName.ToString()},
                {"level", level.ToString()},
                {"message", message}
            };

            if (exception != null)
            {
                _telemetryClient.TrackException(exception, properties);
            }
            _telemetryClient.TrackEvent($"LOG:{level}", properties);
            return Task.FromResult(0);
        }
    }
}
