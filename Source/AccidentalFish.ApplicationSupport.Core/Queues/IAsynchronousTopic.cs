using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Post items on a topic
    /// </summary>
    /// <typeparam name="T">The topic item type</typeparam>
    public interface IAsynchronousTopic<in T> where T : class
    {
        /// <summary>
        /// Send the given object to the topic
        /// </summary>
        /// <param name="payload">Object to post</param>
        /// <param name="messageProperties">Optional properties for the message</param>
        void Send(T payload, IDictionary<string, object> messageProperties = null);
        /// <summary>
        /// Send the given object to the topic
        /// </summary>
        /// <param name="payload">Object to post</param>
        /// <param name="messageProperties">Optional properties for the message</param>
        /// <returns>An awaitable task</returns>
        Task SendAsync(T payload, IDictionary<string, object> messageProperties = null);
    }
}
