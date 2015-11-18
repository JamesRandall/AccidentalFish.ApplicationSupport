using AccidentalFish.ApplicationSupport.Core.Naming;
using Serilog.Core;
using Serilog.Events;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog
{
    internal class FullyQualifiedNameEnricher : ILogEventEnricher
    {
        private readonly IFullyQualifiedName _source;
        private readonly string _propertyName;

        public FullyQualifiedNameEnricher(IFullyQualifiedName source) : this(source, "SourceFqn")
        {
            
        }

        public FullyQualifiedNameEnricher(IFullyQualifiedName source, string propertyName)
        {
            _source = source;
            _propertyName = propertyName;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(_propertyName, _source.FullyQualifiedName));
        }
    }
}
