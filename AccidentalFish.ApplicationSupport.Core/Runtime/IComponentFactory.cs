using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    internal interface IComponentFactory
    {
        IHostableComponent Create(IComponentIdentity componentIdentity);
    }
}
