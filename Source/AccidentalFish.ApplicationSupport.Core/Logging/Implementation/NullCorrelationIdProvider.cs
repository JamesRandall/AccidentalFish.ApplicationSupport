namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class NullCorrelationIdProvider : ICorrelationIdProvider
    {
        public string CorrelationId { get; set; }
    }
}
