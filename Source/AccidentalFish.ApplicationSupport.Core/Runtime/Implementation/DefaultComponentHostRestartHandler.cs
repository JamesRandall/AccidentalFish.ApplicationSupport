using System;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Core.Runtime.Implementation
{
    internal class DefaultComponentHostRestartHandler : IComponentHostRestartHandler
    {
        public async Task<bool> HandleRestart(Exception ex, int retryCount, IAsynchronousLogger logger, IComponentIdentity component)
        {
            try
            {
                bool doDelay = retryCount % 5 == 0;

                if (doDelay)
                {
                    await
                        logger.WarningAsync(
                            $"Error occurred in component {component.FullyQualifiedName}. Restarting in 30 seconds.", ex);
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
                else
                {
                    await logger.WarningAsync($"Error occurred in component {component.FullyQualifiedName}. Restarting immediately.", ex);
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
