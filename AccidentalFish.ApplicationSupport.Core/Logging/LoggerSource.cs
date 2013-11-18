using CuttingEdge.Conditions;

namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    public class LoggerSource : ILoggerSource
    {
        private readonly string _fullyQualifiedName;

        public LoggerSource(string fullyQualifiedName)
        {
            Condition.Requires(fullyQualifiedName).IsNotNullOrWhiteSpace();
            _fullyQualifiedName = fullyQualifiedName;
        }

        public string FullyQualifiedName { get { return _fullyQualifiedName; } }
    }
}
