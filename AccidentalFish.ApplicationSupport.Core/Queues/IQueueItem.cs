namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IQueueItem<out T> where T : class
    {
        T Item { get; }

        int DequeueCount { get; }
    }
}
