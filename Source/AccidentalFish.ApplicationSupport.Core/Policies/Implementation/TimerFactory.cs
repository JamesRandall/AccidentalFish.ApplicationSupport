using System;

namespace AccidentalFish.ApplicationSupport.Core.Policies.Implementation
{
    internal class TimerFactory : ITimerFactory
    {
        private readonly ITimerThreadPoolExecuter _timerThreadPoolExecuter;
        private readonly IAsynchronousDelay _taskDelay;

        public TimerFactory(ITimerThreadPoolExecuter timerThreadPoolExecuter, IAsynchronousDelay taskDelay)
        {
            _timerThreadPoolExecuter = timerThreadPoolExecuter;
            _taskDelay = taskDelay;
        }

        public IAsynchronousIntervalTimer CreateAsynchronousIntervalTimer(TimeSpan interval, bool delayOnExecute = false)
        {
            return new AsynchronousIntervalTimer(_taskDelay, interval, delayOnExecute);
        }

        public IAsynchronousRegularTimer CreateAsynchronousRegularTimer(TimeSpan interval, bool delayOnExecute = false)
        {
            return new AsynchronousRegularTimer(_timerThreadPoolExecuter, _taskDelay, interval, delayOnExecute);
        }
    }
}
