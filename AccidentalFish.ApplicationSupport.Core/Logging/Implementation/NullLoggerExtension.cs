using AccidentalFish.ApplicationSupport.Core.Logging.Model;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class NullLoggerExtension : ILoggerExtension
    {
        public void Logger(LogQueueItem item, bool willLog)
        {
            // do nothing
        }
    }
}
