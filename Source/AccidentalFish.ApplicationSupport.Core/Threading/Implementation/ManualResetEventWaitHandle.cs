using System;
using System.Threading;

namespace AccidentalFish.ApplicationSupport.Core.Threading.Implementation
{
    class ManualResetEventWaitHandle : IWaitHandle, IDisposable
    {
        private readonly ManualResetEvent _manualResetEvent;

        public ManualResetEventWaitHandle()
        {
            _manualResetEvent = new ManualResetEvent(false);
        }

        public bool Wait(int timeout)
        {
            return _manualResetEvent.WaitOne(timeout);
        }

        public bool Wait(TimeSpan timeout)
        {
            return _manualResetEvent.WaitOne(timeout);
        }

        public void Reset()
        {
            _manualResetEvent.Reset();
        }

        public bool IsSet
        {
            get { return _manualResetEvent.WaitOne(0); }
        }

        public void Dispose()
        {
            _manualResetEvent.Dispose();
        }
    }
}
