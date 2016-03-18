using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    interface ITimerThreadPoolExecuter
    {
        Task Run(Action task, CancellationToken taskCancellationToken);
    }
}
