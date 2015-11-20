using System;
using AccidentalFish.ApplicationSupport.Core.Logging;
using Serilog.Events;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog.Implementation
{
    internal static class LogLevelEnumExtensions
    {
        public static LogEventLevel ToLogEventLevel(this LogLevelEnum logLevel)
        {
            switch (logLevel)
            {
                case LogLevelEnum.Verbose: return LogEventLevel.Verbose;
                case LogLevelEnum.Debug: return LogEventLevel.Debug;
                case LogLevelEnum.Information: return LogEventLevel.Information;
                case LogLevelEnum.Warning: return LogEventLevel.Warning;
                case LogLevelEnum.Error: return LogEventLevel.Error;
                case LogLevelEnum.Fatal: return LogEventLevel.Fatal;
            }
            throw new NotSupportedException($"Log level {logLevel} is not supported.");
        }
    }
}
