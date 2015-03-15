using AccidentalFish.ApplicationSupport.Core.Components;
using Microsoft.Practices.Unity;

namespace AccidentalFish.ApplicationSupport.Core.Runtime.Implementation
{
    internal class ComponentFactory : IComponentFactory
    {
        private readonly IUnityContainer _unityContainer;

        public ComponentFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IHostableComponent Create(IComponentIdentity componentIdentity)
        {
            return _unityContainer.Resolve<IHostableComponent>(componentIdentity.ToString());
        }
    }
}
