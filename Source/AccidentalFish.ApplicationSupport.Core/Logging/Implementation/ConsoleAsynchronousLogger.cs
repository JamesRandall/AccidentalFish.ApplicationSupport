using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class ConsoleAsynchronousLogger : IAsynchronousLogger
    {
        private readonly IFullyQualifiedName _source;
        private readonly LogLevelEnum _minimumLogLevel;

        public ConsoleAsynchronousLogger(IFullyQualifiedName source, LogLevelEnum minimumLogLevel)
        {
            _source = source;
            _minimumLogLevel = minimumLogLevel;
        }


        public async Task VerboseAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Verbose, message, additionalData);
        }

        public async Task VerboseAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Verbose, message, exception, additionalData);
        }

        public async Task DebugAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Debug, message, additionalData);
        }

        public async Task DebugAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Debug, message, exception, additionalData);
        }

        public async Task InformationAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Information, message, additionalData);
        }

        public async Task InformationAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Information, message, exception, additionalData);
        }

        public async Task WarningAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Warning, message, additionalData);
        }

        public async Task WarningAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Warning, message, exception, additionalData);
        }

        public async Task ErrorAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Error, message, additionalData);
        }

        public async Task ErrorAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Error, message, exception, additionalData);
        }

        public async Task FatalAsync(string message, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Fatal, message, additionalData);
        }

        public async Task FatalAsync(string message, Exception exception, params object[] additionalData)
        {
            await LogAsync(LogLevelEnum.Fatal, message, exception, additionalData);
        }

        public Task LogAsync(LogLevelEnum level, string message, params object[] additionalData)
        {
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Console.WriteLine($"{source} - {level.ToString().ToUpper()} : {string.Format(message, additionalData)}");
            }
            return Task.FromResult(0);
        }

        public Task LogAsync(LogLevelEnum level, string message, Exception exception, params object[] additionalData)
        {
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Console.WriteLine($"{source} - {level.ToString().ToUpper()} : {exception.GetType().Name} - {string.Format(message,additionalData)}");
            }
            
            return Task.FromResult(0);
        }
    }
}
