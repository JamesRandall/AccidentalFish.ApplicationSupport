using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Logging.AzureQueueLogger.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IFullyQualifiedNameExtensions
    {
        public static bool IsFrameworkSource(this IFullyQualifiedName source)
        {
            if (source == null) return false;
            return source.FullyQualifiedName == "AccidentalFish.ApplicationSupport.Core" ||
                   source.FullyQualifiedName == "AccidentalFish.ApplicationSupport.Repository.EntityFramework" ||
                   source.FullyQualifiedName == "AccidentalFish.ApplicationSupport.Azure";
        }
    }
}
