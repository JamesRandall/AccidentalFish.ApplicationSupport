using System;
using System.Threading;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    public interface IAsynchronousBackoffPolicy
    {
        void Execute(Action<Action<bool>> function, CancellationToken token);
        void Execute(Action<Action<bool>> function, Action shutdownAction, CancellationToken cancellationToken);
    }
}
