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

        public Task LogAsync(LogLevelEnum level, string message)
        {
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Console.WriteLine($"{source} - {level.ToString().ToUpper()} : {message}");
            }
            return Task.FromResult(0);
        }

        public Task LogAsync(LogLevelEnum level, string message, Exception exception)
        {
            if (level >= _minimumLogLevel)
            {
                string source = _source?.FullyQualifiedName ?? "default";
                Console.WriteLine($"{source} - {level.ToString().ToUpper()} : {exception.GetType().Name} - {message}");
            }
            
            return Task.FromResult(0);
        }
    }
}
