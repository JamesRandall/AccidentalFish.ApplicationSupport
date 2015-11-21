using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class ILoggerFactoryExtensions
    {
        public static ILogger GetAssemblyLogger(this ILoggerFactory loggerFactory)
        {
            return loggerFactory.CreateLogger(new LoggerSource("AccidentalFish.ApplicationSupport.Repository.EntityFramework"));
        }
    }
}
