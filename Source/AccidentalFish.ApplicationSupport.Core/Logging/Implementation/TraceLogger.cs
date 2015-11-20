using System;
using System.Diagnostics;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class TraceLogger : ILogger
    {
        private readonly IFullyQualifiedName _source;
        private readonly LogLevelEnum _minimumLogLevel;

        public TraceLogger(IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            _source = source;
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
            Log(LogLevelEnum.Warning, message, additionalData);
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
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Write(level, $"{source} - {level.ToString().ToUpper()} : {string.Format(message, additionalData)}");
            }
        }

        public void Log(LogLevelEnum level, string message, Exception exception, params object[] additionalData)
        {
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Write(level, $"{source} - {level.ToString().ToUpper()} : {exception.GetType().Name} - {string.Format(message, additionalData)}");
            }
        }

        private void Write(LogLevelEnum level, string text)
        {
            if (level == LogLevelEnum.Warning)
            {
                Trace.TraceWarning(text);
            }
            else if (level == LogLevelEnum.Error || level == LogLevelEnum.Fatal)
            {
                Trace.TraceError(text);
            }
            else
            {
                Trace.TraceInformation(text);
            }
        }
    }
}
