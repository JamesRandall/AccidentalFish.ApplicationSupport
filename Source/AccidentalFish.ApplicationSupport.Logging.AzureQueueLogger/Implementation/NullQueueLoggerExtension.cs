using System;
using AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger.Implementation
{
    internal class NullQueueLoggerExtension : IQueueLoggerExtension
    {
        public bool BeforeLog(LogQueueItem item, Exception originalException, bool willLog)
        {
            return true;
        }
    }
}
