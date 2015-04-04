using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using AccidentalFish.ApplicationSupport.Core.Queues;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class LegacyQueueSerializer : IQueueSerializer
    {
        public string Serialize<T>(T item) where T : class
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(memoryStream, item);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public T Deserialize<T>(string item) where T : class
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(item)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(memoryStream, item);
                return (T)serializer.ReadObject(memoryStream);
            }
        }
    }
}
