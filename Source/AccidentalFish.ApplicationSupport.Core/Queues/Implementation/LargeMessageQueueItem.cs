namespace AccidentalFish.ApplicationSupport.Core.Queues.Implementation
{
    internal class LargeMessageQueueItem<T> : IQueueItem<T> where T : class
    {
        public LargeMessageQueueItem(T item, int dequeueCount, IQueueItem<LargeMessageReference> actualQueueItem)
        {
            Item = item;
            DequeueCount = dequeueCount;
            ActualQueueItem = actualQueueItem;
        } 

        public T Item { get; }
        public int DequeueCount { get; }
        public IQueueItem<LargeMessageReference> ActualQueueItem;
    }
}
