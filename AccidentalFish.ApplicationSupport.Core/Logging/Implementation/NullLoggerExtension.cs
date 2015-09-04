using System;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class NullLoggerExtension : ILoggerExtension
    {
        public void Logger(LogQueueItem item, Exception originalException, bool willLog)
        {
            // do nothing
        }
    }
}
