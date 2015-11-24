using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Queues;
using Microsoft.ServiceBus.Messaging;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class AsynchronousTopic<T> : IAsynchronousTopic<T> where T : class
    {
        private readonly IQueueSerializer _queueSerializer;
        private readonly string _connectionString;
        private readonly string _topicName;
        private readonly IAzureAssemblyLogger _logger;
        private readonly TopicClient _client;

        public AsynchronousTopic(
            IQueueSerializer queueSerializer,
            string connectionString,
            string topicName,
            IAzureAssemblyLogger logger)
        {
            _queueSerializer = queueSerializer;
            _connectionString = connectionString;
            _topicName = topicName;
            _logger = logger;
            _client = TopicClient.CreateFromConnectionString(connectionString, topicName);

            _logger?.Verbose("AsynchronousTopic: created for topic {0}", topicName);
        }

        public void Send(T payload)
        {
            string value = _queueSerializer.Serialize(payload);
            BrokeredMessage message = new BrokeredMessage(value);
            _client.Send(message);

            _logger?.Verbose("AsynchronousTopic: Send - placed item on topic {0}", _topicName);
        }

        public async Task SendAsync(T payload)
        {
            string value = _queueSerializer.Serialize(payload);
            BrokeredMessage message = new BrokeredMessage(value);
            await _client.SendAsync(message);

            _logger?.Verbose("AsynchronousTopic: SendAsync - placed item on topic {0}", _topicName);
        }

        internal string ConnectionString => _connectionString;

        internal TopicClient UnderlyingTopic => _client;
    }
}
