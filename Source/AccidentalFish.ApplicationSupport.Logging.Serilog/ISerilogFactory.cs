using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;
using Serilog;
using ILogger=Serilog.ILogger;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog
{
    public interface ISerilogFactory : ILoggerFactory
    {
        ILogger CreateSerilog(LoggerConfiguration loggerConfiguration);
    }
}
