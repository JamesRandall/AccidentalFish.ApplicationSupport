using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class ConsoleLogger : ILogger
    {
        private readonly IFullyQualifiedName _source;
        private readonly LogLevelEnum _minimumLogLevel;

        public ConsoleLogger(IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            _source = source;
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
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Console.WriteLine($"{source} - {level.ToString().ToUpper()} : {message}");
            }
        }

        public void Log(LogLevelEnum level, string message, Exception exception)
        {
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Console.WriteLine($"{source} - {level.ToString().ToUpper()} : {exception.GetType().Name} - {message}");
            }
        }
    }
}
