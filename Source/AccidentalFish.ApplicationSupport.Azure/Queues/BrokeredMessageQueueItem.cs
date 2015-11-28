using System.Collections.ObjectModel;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class BrokeredMessageQueueItem<T> : QueueItem<T> where T : class
    {
        private readonly BrokeredMessage _message;

        public BrokeredMessageQueueItem(BrokeredMessage message, T item, int dequeueCount, string popReceipt)
            : base(item, dequeueCount, popReceipt, new ReadOnlyDictionary<string,object>(message.Properties))
        {
            _message = message;
        }

        public BrokeredMessage Message => _message;
    }
}
