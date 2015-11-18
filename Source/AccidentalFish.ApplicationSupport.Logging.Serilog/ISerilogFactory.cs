using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Naming;
using Serilog;
using ILogger=Serilog.ILogger;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog
{
    public interface ISerilogFactory : ILoggerFactory
    {
        ILogger CreateSerilog(LoggerConfiguration loggerConfiguration);
        ILogger CreateSerilog(LogLevelEnum? minimumLogLevel=null);
        ILogger CreateSerilog(IFullyQualifiedName source, LogLevelEnum? minimumLogLevel=null);
    }
}
