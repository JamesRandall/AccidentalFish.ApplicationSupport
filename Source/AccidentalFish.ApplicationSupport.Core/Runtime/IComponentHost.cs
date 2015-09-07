using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    /// <summary>
    /// Provides a hosting framework for components (typically in services or worker roles)
    /// </summary>
    public interface IComponentHost
    {
        /// <summary>
        /// Optional action to take when there is an error in the component or starting the component. The int parameter is the number of times the
        /// component instance has been restarted
        /// </summary>
        Action<Exception, int> CustomErrorHandler { get; set; }

        /// <summary>
        /// Start the component host with the supplied configuration
        /// </summary>
        /// <param name="configurationProvider">Component host configuration</param>
        /// <param name="cancellationTokenSource">Used to signal when the component host should shut down</param>
        /// <returns>An array of tasks that are hosting components - there will be one task per component instance</returns>
        Task<IEnumerable<Task>> StartAsync(IComponentHostConfigurationProvider configurationProvider, CancellationTokenSource cancellationTokenSource);
        /// <summary>
        /// Stop the component host
        /// </summary>
        void Stop();
    }
}
