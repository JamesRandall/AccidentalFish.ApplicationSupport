using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger.Implementation
{
    internal static class ApplicationResourceFactoryExtensions
    {
        private static readonly IComponentIdentity ApplicationSupportComponentIdentity =new ComponentIdentity(Constants.ApplicationSupportFqn);
        public static IAsynchronousQueue<LogQueueItem> GetAsynchronousLoggerQueue(this IApplicationResourceFactory factory)
        {
            return factory.GetAsyncQueue<LogQueueItem>(factory.Setting(ApplicationSupportComponentIdentity, "logger-queue"), ApplicationSupportComponentIdentity);
        }
        public static IQueue<LogQueueItem> GetLoggerQueue(this IApplicationResourceFactory factory)
        {
            return factory.GetQueue<LogQueueItem>(factory.Setting(ApplicationSupportComponentIdentity, "logger-queue"), ApplicationSupportComponentIdentity);
        }
    }
}
