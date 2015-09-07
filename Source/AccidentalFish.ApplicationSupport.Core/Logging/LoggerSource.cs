using System;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    /// <summary>
    /// Identifies a non-component source for a log item
    /// </summary>
    public class LoggerSource : ILoggerSource
    {
        private readonly string _fullyQualifiedName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fullyQualifiedName">Source name</param>
        public LoggerSource(string fullyQualifiedName)
        {
            if (String.IsNullOrWhiteSpace(fullyQualifiedName)) throw new ArgumentNullException(nameof(fullyQualifiedName));

            _fullyQualifiedName = fullyQualifiedName;
        }

        /// <summary>
        /// Source name
        /// </summary>
        public string FullyQualifiedName => _fullyQualifiedName;
    }
}
