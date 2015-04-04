namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IQueueSerializer
    {
        string Serialize<T>(T item) where T : class;
        T Deserialize<T>(string item) where T : class;
    }
}
