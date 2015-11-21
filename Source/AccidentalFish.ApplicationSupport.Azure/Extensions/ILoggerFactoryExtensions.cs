using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Azure.Extensions
{
    // ReSharper disable once InconsistentNaming
    internal static class ILoggerFactoryExtensions
    {
        public static ILogger GetAssemblyLogger(this ILoggerFactory loggerFactory)
        {
            return loggerFactory.CreateLogger(new LoggerSource("AccidentalFish.ApplicationSupport.Azure"));
        }
    }
}
