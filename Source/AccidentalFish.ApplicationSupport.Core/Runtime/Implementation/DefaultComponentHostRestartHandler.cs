using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Core.Runtime.Implementation
{
    internal class DefaultComponentHostRestartHandler : IComponentHostRestartHandler
    {
        public async Task<bool> HandleRestart(Exception ex, int retryCount, ILogger logger, IComponentIdentity component)
        {
            try
            {
                bool doDelay = retryCount % 5 == 0;

                if (doDelay)
                {
                    logger?.Warning("Error occurred in component {0}. Restarting in 30 seconds.", ex, component.FullyQualifiedName);
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
                else
                {
                    logger?.Warning("Error occurred in component {0}. Restarting immediately.", ex, component.FullyQualifiedName);
                }
            }
            catch (Exception)
            {
                // swallow any issues
            }
            return true;
        }
    }
}
