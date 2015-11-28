using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class CloudQueueItem<T> : QueueItem<T> where T : class
    {
        public CloudQueueItem(CloudQueueMessage message, T item, int dequeueCount, string popReceipt) : base(item, dequeueCount, popReceipt, null)
        {
            CloudQueueMessage = message;
        }

        public CloudQueueMessage CloudQueueMessage { get; set; }
    }
}
