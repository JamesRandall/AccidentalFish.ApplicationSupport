using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IAsynchronousTopic<in T> where T : class
    {
        void Send(T payload);
        Task SendAsync(T payload);
    }
}
