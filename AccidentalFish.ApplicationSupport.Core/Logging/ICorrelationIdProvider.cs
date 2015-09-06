namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    /// <summary>
    /// Implementations provide a correlation ID for tracking requests across services
    /// </summary>
    public interface ICorrelationIdProvider
    {
        /// <summary>
        /// The correlation ID - null if none available
        /// </summary>
        string CorrelationId { get; set; }
    }
}
