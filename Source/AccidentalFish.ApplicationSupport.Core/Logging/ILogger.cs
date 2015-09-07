using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    /// <summary>
    /// The level of the log item
    /// </summary>
    public enum LogLevelEnum
    {
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 1,
        /// <summary>
        /// Informatonal
        /// </summary>
        Information = 2,
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 3,
        /// <summary>
        /// Error
        /// </summary>
        Error = 4
    }

    /// <summary>
    /// Interface for log implementations
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task DebugAsync(string message);

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task DebugAsync(string message, Exception exception);

        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task Information(string message);
        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task Information(string message, Exception exception);

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task Warning(string message);
        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task Warning(string message, Exception exception);

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task Error(string message);
        /// <summary>
        /// Log a error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task Error(string message, Exception exception);

        /// <summary>
        /// Log a message at the specified level
        /// </summary>
        /// <param name="level">The level to log at</param>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task Log(LogLevelEnum level, string message);

        /// <summary>
        /// Log a message at the specified level
        /// </summary>
        /// <param name="level">The level to log at</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task Log(LogLevelEnum level, string message, Exception exception);
    }
}
