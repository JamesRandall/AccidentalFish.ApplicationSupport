namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Serializes and deserializes objects to and from strings for placing on a queue
    /// </summary>
    public interface IQueueSerializer
    {
        /// <summary>
        /// Serialize the item to a string
        /// </summary>
        /// <typeparam name="T">The type of the queue item</typeparam>
        /// <param name="item">The object to serialize</param>
        /// <returns>Serialized string</returns>
        string Serialize<T>(T item) where T : class;
        /// <summary>
        /// Deserialize the item from a string
        /// </summary>
        /// <typeparam name="T">The type of the queue item</typeparam>
        /// <param name="value">The string to deserializa</param>
        /// <returns>Deserialized object</returns>
        T Deserialize<T>(string value) where T : class;
    }
}
