namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public class ComponentConfiguration
    {
        public IComponentIdentity ComponentIdentity { get; set; }

        public int MinimumInstance { get; set; }

        public IComponentIdentity RetryPolicyIdentity { get; set; }
    }
}
