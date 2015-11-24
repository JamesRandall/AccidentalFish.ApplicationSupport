using System;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    internal interface ICoreAssemblyLogger
    {
        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        void Verbose(string message, params object[] additionalData);

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <param name="additionalData">Optional additional data to supply to the logger</param>
        void Verbose(string message, Exception exception, params object[] additionalData);
    }
}
