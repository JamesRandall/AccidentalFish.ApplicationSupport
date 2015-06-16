using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Injection;

namespace AccidentalFish.ApplicationSupport.Core.Runtime.Implementation
{
    internal class ComponentFactory : IComponentFactory
    {
        private readonly IDependencyResolver _container;

        public ComponentFactory(IDependencyResolver container)
        {
            _container = container;
        }

        public IHostableComponent Create(IComponentIdentity componentIdentity)
        {
            return _container.Resolve<IHostableComponent>(componentIdentity.ToString());
        }
    }
}
