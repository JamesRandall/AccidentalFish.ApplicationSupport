using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IAsynchronousQueue<T> where T : class
    {
        Task EnqueueAsync(T item);
        void Enqueue(T item, Action<T> success, Action<T, Exception> failure);
        Task DequeueAsync(Func<T, Task<bool>> processor);
        void Dequeue(Func<T, bool> success, Action<Exception> failure);
        void Dequeue(Func<T, bool> success, Action noMessageAction, Action<Exception> failure);
    }
}
