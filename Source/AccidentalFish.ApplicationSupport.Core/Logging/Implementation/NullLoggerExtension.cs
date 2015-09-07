using System;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class NullLoggerExtension : ILoggerExtension
    {
        public bool BeforeLog(LogQueueItem item, Exception originalException, bool willLog)
        {
            return true;
        }
    }
}
