using AccidentalFish.ApplicationSupport.Core.Queues;
using Newtonsoft.Json;

namespace AccidentalFish.ApplicationSupport.Azure.Queues
{
    internal class QueueSerializer : IQueueSerializer
    {
        public string Serialize<T>(T item) where T : class
        {
            return JsonConvert.SerializeObject(item);
        }

        public T Deserialize<T>(string item) where T : class
        {
            return JsonConvert.DeserializeObject<T>(item);
        }
    }
}
