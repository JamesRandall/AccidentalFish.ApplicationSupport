using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Core.Private
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
