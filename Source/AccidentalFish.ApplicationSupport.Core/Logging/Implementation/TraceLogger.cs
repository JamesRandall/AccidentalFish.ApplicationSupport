using System;
using System.Diagnostics;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    /// <summary>
    /// Logger that writes to the diagnostic tracer
    /// </summary>
    internal class TraceLogger : ILogger
    {
        private readonly IFullyQualifiedName _source;
        private readonly LogLevelEnum _minimumLogLevel;

        /// <summary>
        /// Constructor
        /// </summary>
        public TraceLogger(IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            _source = source;
            _minimumLogLevel = minimumLogLevel;
        }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Verbose(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Verbose, message, additionalData);
        }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Verbose(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Verbose, message, exception, additionalData);
        }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Debug(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Debug, message, additionalData);
        }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Debug(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Debug, message, exception, additionalData);
        }

        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Information(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Information, message, additionalData);
        }

        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Information(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Information, message, exception, additionalData);
        }

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Warning(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Warning, message, additionalData);
        }

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Warning(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Warning, message, exception, additionalData);
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Error(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Error, message, additionalData);
        }

        /// <summary>
        /// Log a error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Error(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Error, message, exception, additionalData);
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Fatal(string message, params object[] additionalData)
        {
            Log(LogLevelEnum.Fatal, message, additionalData);
        }
        
        /// <summary>
        /// Log a error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Fatal(string message, Exception exception, params object[] additionalData)
        {
            Log(LogLevelEnum.Fatal, message, exception, additionalData);
        }

        /// <summary>
        /// Log a message at the specified level
        /// </summary>
        /// <param name="level">The level to log at</param>
        /// <param name="message">The message to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        public void Log(LogLevelEnum level, string message, params object[] additionalData)
        {
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Write(level, $"{source} - {level.ToString().ToUpper()} : {string.Format(message, additionalData)}");
            }
        }

        /// <summary>
        /// Log a message at the specified level
        /// </summary>
        /// <param name="level">The level to log at</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
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
