using System;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// Instantiates various form of timer
    /// </summary>
    public interface ITimerFactory
    {
        /// <summary>
        /// Creates an interval based timer that executes a task. When the task is completed the timer waits for the specified interval then executes it again.
        /// </summary>
        /// <param name="interval">The size of the interval</param>
        /// <param name="delayOnExecute">If true then when execute is first called the timer waits for the specified interval before first executing the task. Defaults to false.</param>
        /// <returns>An interval based timer</returns>
        IAsynchronousIntervalTimer CreateAsynchronousIntervalTimer(TimeSpan interval, bool delayOnExecute = false);
        /// <summary>
        /// Creates an timer that executes a task metronomically (i.e. every n seconds) regardless of the duration of the task. Note that tasks running under this timer will be permitted to run
        /// no longer than the interval the timer was created with - should they overrun then a cancellation will be triggered.
        /// </summary>
        /// <param name="interval">Schedule for the timer</param>
        /// <param name="delayOnExecute">If true then when execute is first called the timer waits for the specified interval before first executing the task. Defaults to false.</param>
        /// <returns>An regular timer</returns>
        IAsynchronousRegularTimer CreateAsynchronousRegularTimer(TimeSpan interval, bool delayOnExecute = false);
    }
}