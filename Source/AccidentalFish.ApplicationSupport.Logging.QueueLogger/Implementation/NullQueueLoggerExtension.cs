using System;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation
{
    internal class NullQueueLoggerExtension : IQueueLoggerExtension
    {
        public bool BeforeLog(LogQueueItem item, Exception originalException, bool willLog)
        {
            return true;
        }
    }
}
