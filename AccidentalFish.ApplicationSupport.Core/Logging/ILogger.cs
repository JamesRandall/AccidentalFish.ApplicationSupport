using System;

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
        void Debug(string message);
        void Debug(string message, Exception exception);

        void Information(string message);
        void Information(string message, Exception exception);

        void Warning(string message);
        void Warning(string message, Exception exception);

        void Error(string message);
        void Error(string message, Exception exception);

        void Log(LogLevelEnum level, string message);
        void Log(LogLevelEnum level, string message, Exception exception);
    }
}
