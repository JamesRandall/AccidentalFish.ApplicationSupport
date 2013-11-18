namespace AccidentalFish.ApplicationSupport.Core.Threading
{
    public interface IWaitHandle
    {
        bool Wait(int timeout);
        void Reset();
        bool IsSet { get; }
    }
}
