using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class ConsoleLogger : ILogger
    {
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

        public Task Log(LogLevelEnum level, string message)
        {
            Console.WriteLine("{0}: {1}", level.ToString().ToUpper(), message);
            return Task.FromResult(0);
        }

        public Task Log(LogLevelEnum level, string message, Exception exception)
        {
            Console.WriteLine("{0} {1}: {2}", level.ToString().ToUpper(), exception.GetType().Name, message);
            return Task.FromResult(0);
        }
    }
}
