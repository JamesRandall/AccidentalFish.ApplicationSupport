using System;
using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Provides timings for back off policies instantiated outside of a factory
    /// </summary>
    public interface IBackoffPolicyTimingProvider
    {
        /// <summary>
        /// Get the default timings
        /// </summary>
        /// <returns>A read only collection of timespans</returns>
        IReadOnlyCollection<TimeSpan> GetIntervals();
    }
}
