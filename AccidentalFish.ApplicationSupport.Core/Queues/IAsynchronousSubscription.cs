using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    public interface IAsynchronousSubscription<out T> where T : class
    {
        /// <summary>
        /// Looks on the subscription for a message
        /// </summary>
        /// <param name="process">Called if a message is found. The function should return true if it wants the message completed, false if abandoned.</param>
        /// <returns>True if a message was found, false if not.</returns>
        Task<bool> Recieve(Func<T, Task<bool>> process);
    }
}
