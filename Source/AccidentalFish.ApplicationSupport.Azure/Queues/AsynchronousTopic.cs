using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class AsynchronousTopic<T> : IAsynchronousTopic<T> where T : class
    {
        private readonly IQueueSerializer _queueSerializer;
        private readonly TopicClient _client;

        public AsynchronousTopic(IQueueSerializer queueSerializer, string connectionString, string topicName)
        {
            _queueSerializer = queueSerializer;
            _client = TopicClient.CreateFromConnectionString(connectionString, topicName);
        }

        public void Send(T payload)
        {
            string value = _queueSerializer.Serialize(payload);
            BrokeredMessage message = new BrokeredMessage(value);
            _client.Send(message);
        }

        public async Task SendAsync(T payload)
        {
            string value = _queueSerializer.Serialize(payload);
            BrokeredMessage message = new BrokeredMessage(value);
            await _client.SendAsync(message);
        }
    }
}
