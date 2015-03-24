using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class BrokeredMessageQueueItem<T> : QueueItem<T> where T : class
    {
        private readonly BrokeredMessage _message;

        public BrokeredMessageQueueItem(BrokeredMessage message, T item, int dequeueCount, string popReceipt) : base(item, dequeueCount, popReceipt)
        {
            _message = message;
        }

        public BrokeredMessage Message { get { return _message; } }
    }
}
