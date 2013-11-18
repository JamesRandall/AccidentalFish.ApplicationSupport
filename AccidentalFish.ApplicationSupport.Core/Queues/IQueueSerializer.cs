namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IQueueSerializer<T> where T : class
    {
        string Serialize(T item);
        T Deserialize(string item);
    }
}
