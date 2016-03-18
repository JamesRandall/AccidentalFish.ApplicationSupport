using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class AsynchronousDelay : IAsynchronousDelay
    {
        public Task Delay(TimeSpan interval)
        {
            return Task.Delay(interval);
        }

        public Task Delay(TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.Delay(interval, cancellationToken);
        }
    }
}
