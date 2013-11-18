using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class QueueSerializer<T> : IQueueSerializer<T> where T : class
    {
        private readonly DataContractJsonSerializer _serializer;

        public QueueSerializer()
        {
            _serializer = new DataContractJsonSerializer(typeof(T));
        }

        public string Serialize(T item)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                _serializer.WriteObject(memoryStream, item);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public T Deserialize(string item)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(item)))
            {
                return (T)_serializer.ReadObject(memoryStream);
            }
        }
    }
}
