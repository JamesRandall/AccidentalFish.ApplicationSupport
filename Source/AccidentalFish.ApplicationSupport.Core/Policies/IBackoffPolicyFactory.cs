using System;
using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Factory for creating back off policies with custom timings
    /// </summary>
    public interface IBackoffPolicyFactory
    {
        /// <summary>
        /// Creates an asynchronous back off policy with the optionally supplied back off intervals
        /// </summary>
        /// <param name="backoffTimings">Optional, defaults to (in ms) 100, 250, 500, 1000, 5000</param>
        /// <returns>A back off policy</returns>
        IAsynchronousBackoffPolicy CreateAsynchronousBackoffPolicy(IReadOnlyCollection<TimeSpan> backoffTimings = null);
        /// <summary>
        /// Creates an synchronous back off policy with the optionally supplied back off intervals
        /// </summary>
        /// <param name="backoffTimings">Optional, defaults to (in ms) 100, 250, 500, 1000, 5000</param>
        /// <returns>A back off policy</returns>
        IBackoffPolicy CreateBackoffPolicy(IReadOnlyCollection<TimeSpan> backoffTimings = null);
    }
}
