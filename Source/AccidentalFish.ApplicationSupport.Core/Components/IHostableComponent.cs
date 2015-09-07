using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// A hostable component is designed to be run in a worker role and perform background activities
    /// </summary>
    public interface IHostableComponent : IApplicationComponent
    {
        /// <summary>
        /// Starts the component
        /// </summary>
        /// <param name="token"></param>
        /// <returns>The task the component is running in</returns>
        Task StartAsync(CancellationToken token);
    }
}
