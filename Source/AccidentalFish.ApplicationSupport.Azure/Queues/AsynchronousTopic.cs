using System.Collections.Generic;
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

        public void Send(T payload, IDictionary<string, object> messageProperties = null)
        {
            string value = _queueSerializer.Serialize(payload);
            BrokeredMessage message = new BrokeredMessage(value);
            AddProperties(message, messageProperties);
            _client.Send(message);

            _logger?.Verbose("AsynchronousTopic: Send - placed item on topic {0}", _topicName);
        }

        public async Task SendAsync(T payload, IDictionary<string, object> messageProperties = null)
        {
            string value = _queueSerializer.Serialize(payload);
            BrokeredMessage message = new BrokeredMessage(value);
            AddProperties(message, messageProperties);
            await _client.SendAsync(message);

            _logger?.Verbose("AsynchronousTopic: SendAsync - placed item on topic {0}", _topicName);
        }

        private static void AddProperties(BrokeredMessage message, IDictionary<string, object> messageProperties)
        {
            if (messageProperties != null)
            {
                foreach (KeyValuePair<string, object> kvp in messageProperties)
                {
                    message.Properties.Add(kvp);
                }
            }
        }

        internal string ConnectionString => _connectionString;

        internal TopicClient UnderlyingTopic => _client;
    }
}
