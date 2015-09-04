using System;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    public interface ILoggerExtension
    {
        void Logger(LogQueueItem item, Exception originalException, bool willLog);
    }
}
