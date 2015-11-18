using AccidentalFish.ApplicationSupport.Core.Logging;
using Serilog.Core;
using Serilog.Events;

namespace AccidentalFish.ApplicationSupport.Logging.Serilog
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly string _propertyName;

        public CorrelationIdEnricher(ICorrelationIdProvider correlationIdProvider)
            : this(correlationIdProvider, "CorrelationId")
        {
            
        }


        public CorrelationIdEnricher(ICorrelationIdProvider correlationIdProvider, string propertyName)
        {
            _correlationIdProvider = correlationIdProvider;
            _propertyName = propertyName;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(_propertyName, _correlationIdProvider.CorrelationId));
        }
    }
}
