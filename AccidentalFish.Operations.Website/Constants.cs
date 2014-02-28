using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.Operations.Website
{
    public class Constants
    {
        public const string WebsiteFqn = "com.accidental-fish.operations.website";
        public readonly static IComponentIdentity WebsiteComponentIdentity = new ComponentIdentity(WebsiteFqn);
    }
}