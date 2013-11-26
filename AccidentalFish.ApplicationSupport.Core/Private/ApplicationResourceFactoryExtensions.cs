using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging.Model;
using AccidentalFish.ApplicationSupport.Core.Queues;

// ReSharper disable once CheckNamespace
namespace AccidentalFish.ApplicationSupport.Private
{
    internal static class ApplicationResourceFactoryExtensions
    {
        private static readonly IComponentIdentity ApplicationSupportComponentIdentity =new ComponentIdentity(Constants.ApplicationSupportFqn);
        public static IAsynchronousQueue<LogQueueItem> GetLoggerQueue(this IApplicationResourceFactory factory)
        {
            return factory.GetQueue<LogQueueItem>(factory.Setting(ApplicationSupportComponentIdentity, "logger-queue"), ApplicationSupportComponentIdentity);
        }
    }
}
