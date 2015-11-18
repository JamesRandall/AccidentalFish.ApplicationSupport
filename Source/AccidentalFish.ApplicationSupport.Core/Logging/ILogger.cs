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
        /// Verbose
        /// </summary>
        Verbose = 1,
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 2,
        /// <summary>
        /// Informatonal
        /// </summary>
        Information = 3,
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 4,
        /// <summary>
        /// Error
        /// </summary>
        Error = 5,
        /// <summary>
        /// Fatal
        /// </summary>
        Fatal = 6
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
        Task VerboseAsync(string message);

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task VerboseAsync(string message, Exception exception);

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
        Task InformationAsync(string message);
        /// <summary>
        /// Log an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task InformationAsync(string message, Exception exception);

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task WarningAsync(string message);
        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task WarningAsync(string message, Exception exception);

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task ErrorAsync(string message);
        /// <summary>
        /// Log a error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task ErrorAsync(string message, Exception exception);

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task FatalAsync(string message);
        /// <summary>
        /// Log a error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task FatalAsync(string message, Exception exception);

        /// <summary>
        /// Log a message at the specified level
        /// </summary>
        /// <param name="level">The level to log at</param>
        /// <param name="message">The message to log</param>
        /// <returns>An awaitable task</returns>
        Task LogAsync(LogLevelEnum level, string message);

        /// <summary>
        /// Log a message at the specified level
        /// </summary>
        /// <param name="level">The level to log at</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">An exception to log</param>
        /// <returns>An awaitable task</returns>
        Task LogAsync(LogLevelEnum level, string message, Exception exception);
    }

    /// <summary>
    /// Log implementations that are actually wrappers for underlying implementations can
    /// implement this interface to expose a method that allows consumers to get to the underlying
    /// logger if required.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILogger<T> : ILogger
    {
        /// <summary>
        /// The underlying logger
        /// </summary>
        T ActualLogger { get; }
    }
}
