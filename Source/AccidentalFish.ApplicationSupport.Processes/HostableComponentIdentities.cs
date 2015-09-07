using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Processes
{
    public static class HostableComponentIdentities
    {
        public static readonly IComponentIdentity LogQueueProcessor = new ComponentIdentity(HostableComponentNames.LogQueueProcessor);
        public static readonly IComponentIdentity EmailQueueProcessor = new ComponentIdentity(HostableComponentNames.EmailQueueProcessor);
    }
}
