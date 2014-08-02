using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class QueueItem<T> : IQueueItem<T> where T : class
    {
        public QueueItem(T item, int dequeueCount, string popReceipt)
        {
            Item = item;
            DequeueCount = dequeueCount;
            PopReciept = popReceipt;
        }
 
        public T Item { get; set; }
        public int DequeueCount { get; set; }
        public string PopReciept { get; set; }
    }
}
