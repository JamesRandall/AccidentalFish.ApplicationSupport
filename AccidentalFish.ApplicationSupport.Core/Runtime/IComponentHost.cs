using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    public interface IComponentHost
    {
        Action<Exception, int> CustomErrorHandler { get; set; }

        Task<IEnumerable<Task>> Start(IComponentHostConfigurationProvider configurationProvider, CancellationTokenSource cancellationTokenSource);

        void Stop();
    }
}
