namespace AccidentalFish.ApplicationSupport.Core.Logging
{
    public interface ICorrelationIdProvider
    {
        string CorrelationId { get; set; }
    }
}
