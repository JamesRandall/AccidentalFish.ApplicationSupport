using System;
using System.Collections.Generic;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using Microsoft.ApplicationInsights;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights
{
    internal class ApplicationInsightLogger : ILogger<TelemetryClient>
    {
        private readonly IFullyQualifiedName _fullyQualifiedName;
        private readonly LogLevelEnum _minimumLogLevel;
        private readonly TelemetryClient _telemetryClient = new TelemetryClient();

        public ApplicationInsightLogger(IFullyQualifiedName fullyQualifiedName, LogLevelEnum minimumLogLevel)
        {
            _fullyQualifiedName = fullyQualifiedName;
            _minimumLogLevel = minimumLogLevel;
        }

        public void Verbose(string message)
        {
            Log(LogLevelEnum.Verbose, message);
        }

        public void Verbose(string message, Exception exception)
        {
            Log(LogLevelEnum.Verbose, message, exception);
        }

        public void Debug(string message)
        {
            Log(LogLevelEnum.Debug, message);
        }

        public void Debug(string message, Exception exception)
        {
            Log(LogLevelEnum.Debug, message, exception);
        }

        public void Information(string message)
        {
            Log(LogLevelEnum.Information, message);
        }

        public void Information(string message, Exception exception)
        {
            Log(LogLevelEnum.Information, message, exception);
        }

        public void Warning(string message)
        {
            Log(LogLevelEnum.Warning, message);
        }

        public void Warning(string message, Exception exception)
        {
            Log(LogLevelEnum.Warning, message, exception);
        }

        public void Error(string message)
        {
            Log(LogLevelEnum.Error, message);
        }

        public void Error(string message, Exception exception)
        {
            Log(LogLevelEnum.Error, message, exception);
        }

        public void Fatal(string message)
        {
            Log(LogLevelEnum.Fatal, message);
        }

        public void Fatal(string message, Exception exception)
        {
            Log(LogLevelEnum.Fatal, message, exception);
        }

        public void Log(LogLevelEnum level, string message)
        {
            Log(level, message, null);
        }

        public void Log(LogLevelEnum level, string message, Exception exception)
        {
            if (level < _minimumLogLevel) return;

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
        }

        public TelemetryClient ActualLogger => _telemetryClient;
    }
}
