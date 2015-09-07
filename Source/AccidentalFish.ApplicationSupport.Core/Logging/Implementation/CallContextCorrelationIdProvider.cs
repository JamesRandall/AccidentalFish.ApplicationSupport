using System.Runtime.Remoting.Messaging;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class CallContextCorrelationIdProvider : ICorrelationIdProvider
    {
        private readonly string _callContextKey;

        public CallContextCorrelationIdProvider(string callContextKey)
        {
            _callContextKey = callContextKey;
        }

        public string CorrelationId
        {
            get
            {
                object correlationId = CallContext.LogicalGetData(_callContextKey);
                if (correlationId == null)
                {
                    return null;
                }
                return (string)correlationId;
            }
            set
            {
                CallContext.LogicalSetData(_callContextKey, value);
            }
        }
    }
}
