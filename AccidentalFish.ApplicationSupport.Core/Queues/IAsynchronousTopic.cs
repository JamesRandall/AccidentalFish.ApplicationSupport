using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IAsynchronousTopic<in T> where T : class
    {
        Task SendAsync(T payload);
    }
}
