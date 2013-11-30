using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    public interface IComponentHost
    {
        Task<IEnumerable<Task>> Start(IComponentHostConfigurationProvider configurationProvider, CancellationTokenSource cancellationTokenSource);

        void Stop();
    }
}
