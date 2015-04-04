using System;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    public class LoggerSource : ILoggerSource
    {
        private readonly string _fullyQualifiedName;

        public LoggerSource(string fullyQualifiedName)
        {
            if (String.IsNullOrWhiteSpace(fullyQualifiedName)) throw new ArgumentNullException("fullyQualifiedName");

            _fullyQualifiedName = fullyQualifiedName;
        }

        public string FullyQualifiedName { get { return _fullyQualifiedName; } }
    }
}
