using System.Threading;
using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    public interface IComponentHost
    {
        void Start(IComponentHostConfigurationProvider configurationProvider, CancellationTokenSource cancellationTokenSource);

        void Stop();
    }
}
