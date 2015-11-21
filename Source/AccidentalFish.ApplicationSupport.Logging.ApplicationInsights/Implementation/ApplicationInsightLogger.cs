using System;
using System.Collections.Generic;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using Microsoft.ApplicationInsights;

namespace AccidentalFish.ApplicationSupport.Logging.ApplicationInsights.Implementation
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

        public void Verbose(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Verbose, message, additionalData);
        }

        public void Verbose(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Verbose, message, exception, additionalData);
        }

        public void Debug(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Debug, message, additionalData);
        }

        public void Debug(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Debug, message, exception, additionalData);
        }

        public void Information(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Information, message, additionalData);
        }

        public void Information(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Information, message, exception, additionalData);
        }

        public void Warning(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Warning, message);
        }

        public void Warning(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Warning, message, exception, additionalData);
        }

        public void Error(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Error, message, additionalData);
        }

        public void Error(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Error, message, exception, additionalData);
        }

        public void Fatal(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Fatal, message, additionalData);
        }

        public void Fatal(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Fatal, message, exception, additionalData);
        }

        public void Log(LogLevelEnum level, string message, params object[] additionalData)
        {
            Log(level, message, null, additionalData, additionalData);
        }

        public void Log(LogLevelEnum level, string message, Exception exception, params object[] additionalData)
        {
            if (level < _minimumLogLevel) return;

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
        }

        public TelemetryClient ActualLogger => _telemetryClient;
    }
}
