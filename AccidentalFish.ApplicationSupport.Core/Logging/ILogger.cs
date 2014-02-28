using System;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    public enum LogLevelEnum
    {
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4
    }

    public interface ILogger
    {
        Task Debug(string message);
        Task Debug(string message, Exception exception);

        Task Information(string message);
        Task Information(string message, Exception exception);

        Task Warning(string message);
        Task Warning(string message, Exception exception);

        Task Error(string message);
        Task Error(string message, Exception exception);

        Task Log(LogLevelEnum level, string message);
        Task Log(LogLevelEnum level, string message, Exception exception);
    }
}
