using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Queues
{
    /// <summary>
    /// Intetrface for processing subscriptions
    /// </summary>
    /// <typeparam name="T">Type of the subscription items</typeparam>
    public interface IAsynchronousSubscription<out T> where T : class
    {
        /// <summary>
        /// Looks on the subscription for a message
        /// </summary>
        /// <param name="process">Called if a message is found. The function should return true if it wants the message completed, false if abandoned.</param>
        /// <returns>True if a message was found, false if not.</returns>
        Task<bool> RecieveAsync(Func<T, Task<bool>> process);
    }
}
